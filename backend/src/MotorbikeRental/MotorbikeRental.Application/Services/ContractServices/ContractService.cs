using AutoMapper;
using MotorbikeRental.Application.DTOs.ContractDto;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IServices.IContractServices;
using MotorbikeRental.Application.Interface.IValidators.IContractValidators;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.ContractEnum;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;

namespace MotorbikeRental.Application.Services.ContractServices
{
    public class ContractService : IContractService
    {
        private readonly IMotorbikeRepository motorbikeRepository;
        private readonly IDiscountRepository discountRepository;
        private readonly IContractValidator contractValidator;
        private readonly ICustomerRepository customerRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IRentalContractRepository rentalContractRepository;
        private readonly IMapper mapper;
        public ContractService(IMotorbikeRepository motorbikeRepository, IDiscountRepository discountRepository, IContractValidator contractValidator, ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IRentalContractRepository rentalContractRepository, IMapper mapper)
        {
            this.motorbikeRepository = motorbikeRepository;
            this.discountRepository = discountRepository;
            this.contractValidator = contractValidator;
            this.customerRepository = customerRepository;
            this.employeeRepository = employeeRepository;
            this.rentalContractRepository = rentalContractRepository;
            this.mapper = mapper;
        }
        public async Task<RentalPriceResponseDto> CalculateRentalPrice(RentalPriceRequestDto priceRequestDto, CancellationToken cancellationToken = default)
        {
            if (priceRequestDto.ExpectedReturnDate < priceRequestDto.RentalDate)
                throw new BusinessRuleException("Return date not greater than rental date");
            Motorbike motorbike = await motorbikeRepository.GetByIdWithIncludes(priceRequestDto.MotorbikeId, cancellationToken) ?? throw new NotFoundException($"Motorbike with id {priceRequestDto.MotorbikeId} not found");
            Discount? discount = null;
            if (priceRequestDto.DiscountId != null)
                discount = await discountRepository.GetDiscountById(priceRequestDto.DiscountId.Value, false, cancellationToken) ?? throw new NotFoundException($"Discount with id {priceRequestDto.DiscountId} not found");
            contractValidator.ValidateForCalculateRentalPrice(motorbike, discount, cancellationToken);
            return MapToRentalPriceResponseDto(priceRequestDto, motorbike, discount);
        }
        public async Task<ContractDto> CreateContract(ContractCreateDto contractCreate, CancellationToken cancellation = default)
        {
            Motorbike motorbike = await motorbikeRepository.GetByIdWithIncludes(contractCreate.MotorbikeId) ?? throw new NotFoundException($"Motorbike with id {contractCreate.MotorbikeId} not found");
            Customer customer = await customerRepository.GetCustomerBasicInfoById(contractCreate.CustomerId, cancellation) ?? throw new NotFoundException($"Customer with id {contractCreate.CustomerId} not found");
            Employee employee = await employeeRepository.GetEmployeeBasicInfoById(contractCreate.EmployeeId, cancellation) ?? throw new NotFoundException($"Employee with id {contractCreate.EmployeeId} not found");
            Discount? discount = contractCreate.DiscountId != null ? await discountRepository.GetDiscountById(contractCreate.DiscountId.Value, false, cancellation) ?? throw new NotFoundException($"Discount with id {contractCreate.DiscountId} not found") : null;

            (decimal basePrice, decimal? discountAmount) = GetPriceWithDiscount(contractCreate.RentalDate,
                contractCreate.ExpectedReturnDate,
                contractCreate.RentalTypeStatus,
                contractCreate.RentalTypeStatus == RentalTypeStatus.Hourly ? motorbike.PriceList.HourlyRate : motorbike.PriceList.DailyRate,
                discount != null ? discount.Value : null);

            contractValidator.ValidateForCreate(contractCreate, motorbike, discount, basePrice, discountAmount);

            RentalContract contract = mapper.Map<RentalContract>(contractCreate);
            contract.DiscountAmount = discountAmount;
            contract.DepositAmount = motorbike.Category.DepositAmount;

            await rentalContractRepository.Create(contract, cancellation);

            Motorbike motorbike1 = await motorbikeRepository.GetById(contractCreate.MotorbikeId);
            if (contract.RentalContractStatus == RentalContractStatus.Active)
                motorbike1.Status = MotorbikeStatus.Rented;
            if (contract.RentalContractStatus == RentalContractStatus.Pending)
                motorbike1.Status = MotorbikeStatus.Reserved;
            await motorbikeRepository.SaveChangeAsync(cancellation);

            return MapToContractDto(contract, motorbike, customer, employee);
        }
        public async Task<bool> UpdateContractStatusActive(int contractId, CancellationToken cancellationToken = default)
        {
            RentalContract rentalContract = await rentalContractRepository.GetById(contractId, cancellationToken) ?? throw new NotFoundException($"Contract with id {contractId} not found");
            if(rentalContract.RentalDate > DateTime.UtcNow.AddHours(2))
                throw new BusinessRuleException($"Contract with id {contractId} cannot be activated because the rental date is more than 2 hours in the future");
            if(rentalContract.RentalContractStatus != RentalContractStatus.Pending && rentalContract.RentalContractStatus != RentalContractStatus.Active)
                throw new BusinessRuleException($"Contract with id {contractId} is not in pending status, cannot update to active status");
            if(rentalContract.RentalDate > DateTime.UtcNow.AddHours(10))
                throw new BusinessRuleException($"Contract with id {contractId} cannot be activated because the rental date is more than 10 hours in the future");
            Motorbike motorbike = await motorbikeRepository.GetById(rentalContract.MotorbikeId, cancellationToken);
            if (rentalContract.RentalContractStatus != RentalContractStatus.Pending)
                throw new BusinessRuleException($"Contract with id {contractId} is not in pending status, cannot update to active status");
            rentalContract.RentalContractStatus = RentalContractStatus.Active;
            motorbike.Status = MotorbikeStatus.Rented;
            await rentalContractRepository.SaveChangeAsync(cancellationToken);
            return true;
        }
        public async Task<bool> CancelContractByCustomer(int contractId, CancellationToken cancellationToken = default)
        {
            RentalContract rentalContract = await rentalContractRepository.GetById(contractId, cancellationToken) ?? throw new NotFoundException($"Contract with id {contractId} not found");
            Motorbike motorbike = await motorbikeRepository.GetById(rentalContract.MotorbikeId, cancellationToken);
            if (rentalContract.RentalContractStatus != RentalContractStatus.Pending)
                throw new BusinessRuleException($"Contract with id {contractId} is not in pending status, cannot cancel");
            rentalContract.RentalContractStatus = RentalContractStatus.Cancelled;   
            motorbike.Status = MotorbikeStatus.Available;
            await rentalContractRepository.SaveChangeAsync(cancellationToken);
            return true;
        }
        public async Task<ContractDto> UpdateContractBeforeActivation(ContractUpdateBeforeActivationDto contractUpdate, CancellationToken cancellationToken = default)
        {
            RentalContract rentalContract = await rentalContractRepository.GetContractById(contractUpdate.ContractId, cancellationToken) ?? throw new NotFoundException($"Contract with id {contractUpdate.ContractId} not found");
            Motorbike motorbike = await motorbikeRepository.GetByIdWithIncludes(contractUpdate.MotorbikeId, cancellationToken) ?? throw new NotFoundException($"Motorbike with id {rentalContract.MotorbikeId} not found");
            Discount? discount = contractUpdate.DiscountId != null ? await discountRepository.GetDiscountById(contractUpdate.DiscountId.Value, false, cancellationToken) ?? throw new NotFoundException($"Discount with id {contractUpdate.DiscountId} not found") : null;
            (decimal basePrice, decimal? discountAmount) = GetPriceWithDiscount(contractUpdate.RentalDate,
                contractUpdate.ExpectedReturnDate,
                contractUpdate.RentalTypeStatus,
                contractUpdate.RentalTypeStatus == RentalTypeStatus.Hourly ? motorbike.PriceList.HourlyRate : motorbike.PriceList.DailyRate,
                discount != null ? discount.Value : null);
            contractValidator.ValidateForUpdateBeforeActivation(contractUpdate, rentalContract, motorbike, discount, basePrice, discountAmount);
            mapper.Map(contractUpdate, rentalContract);
            rentalContract.DiscountAmount = discountAmount;
            rentalContract.DepositAmount = motorbike.Category.DepositAmount;
            await rentalContractRepository.Update(rentalContract, cancellationToken);
            Motorbike motorbike1 = await motorbikeRepository.GetById(contractUpdate.MotorbikeId, cancellationToken);
            if (rentalContract.RentalContractStatus == RentalContractStatus.Active)
                motorbike1.Status = MotorbikeStatus.Rented;
            if (rentalContract.RentalContractStatus == RentalContractStatus.Pending)
                motorbike1.Status = MotorbikeStatus.Reserved;
            await motorbikeRepository.SaveChangeAsync(cancellationToken);
            Customer? customer = await customerRepository.GetCustomerBasicInfoById(rentalContract.CustomerId.Value, cancellationToken);
            Employee? employee = await employeeRepository.GetEmployeeBasicInfoById(rentalContract.EmployeeId, cancellationToken);
            ContractDto contractDto = MapToContractDto(rentalContract, motorbike, customer, employee);
            return contractDto;
        }

        public async Task<ContractDto> GetContractById(int contractId, CancellationToken cancellationToken = default)
        {
            RentalContract rentalContract = await rentalContractRepository.GetContractById(contractId, cancellationToken)
                ?? throw new NotFoundException($"Contract with id {contractId} not found");
            Motorbike? motorbike = await motorbikeRepository.GetByIdWithIncludes(rentalContract.MotorbikeId.Value, cancellationToken);
            Employee? employee = await employeeRepository.GetEmployeeBasicInfoById(rentalContract.EmployeeId, cancellationToken);
            Customer? customer = await customerRepository.GetCustomerBasicInfoById(rentalContract.CustomerId.Value, cancellationToken);
            return MapToContractDto(rentalContract, motorbike, customer, employee);
        }
        public async Task<bool> DeleteContract(int contractId, CancellationToken cancellationToken = default)
        {
            RentalContract rentalContract = await rentalContractRepository.GetById(contractId, cancellationToken)
                ?? throw new NotFoundException($"Contract with id {contractId} not found");
            if (rentalContract.RentalContractStatus == RentalContractStatus.Active)
                throw new BusinessRuleException($"Cannot delete contract with id {contractId} because it is currently active");
            if (rentalContract.RentalContractStatus == RentalContractStatus.Pending)
                throw new BusinessRuleException($"Cannot delete contract with id {contractId} because it is currently pending");
            await rentalContractRepository.Delete(rentalContract, cancellationToken);
            return true;
        }

        public async Task<ContractDto> ContractSettlement(ContractSettlementDto contractSettlementDto, CancellationToken cancellationToken = default)
        {
            RentalContract contract = await rentalContractRepository.GetById(contractSettlementDto.ContractId, cancellationToken)
                ?? throw new NotFoundException($"Contract with id {contractSettlementDto.ContractId} not found");
            if (contract.RentalContractStatus != RentalContractStatus.Active)
                throw new BusinessRuleException($"Contract with id {contractSettlementDto.ContractId} is not active, cannot settle");
            Motorbike? motorbike = await motorbikeRepository.GetByIdWithIncludes(contract.MotorbikeId.Value, cancellationToken);
            Employee? employee = await employeeRepository.GetEmployeeBasicInfoById(contract.EmployeeId, cancellationToken);
            Customer? customer = await customerRepository.GetCustomerBasicInfoById(contract.CustomerId.Value, cancellationToken);
            decimal? lateReturnFee = null;
            if (contractSettlementDto.ActualReturnDate > contract.ExpectedReturnDate)
                lateReturnFee = CalculateLateReturnFee(contractSettlementDto.ActualReturnDate, contract.ExpectedReturnDate, contract.LateReturnFeeMultiplier.Value, motorbike.PriceList.HourlyRate);
            contract.ActualReturnDate = contractSettlementDto.ActualReturnDate;
            contract.RentalContractStatus = RentalContractStatus.Completed;
            contract.IdCardHeld = false; 
            await rentalContractRepository.Update(contract, cancellationToken);
            Motorbike motorbike1 = await motorbikeRepository.GetById(contract.MotorbikeId.Value, cancellationToken);
            motorbike1.Status = MotorbikeStatus.Available;
            await motorbikeRepository.SaveChangeAsync(cancellationToken);
            ContractDto contractDto = MapToContractDto(contract, motorbike, customer, employee);
            contractDto.ActualReturnDate = contractSettlementDto.ActualReturnDate;
            contractDto.LateReturnFee = lateReturnFee;
            contractDto.RentalContractStatus = RentalContractStatus.Completed;
            return contractDto;
        }
        public async Task<PaginatedDataDto<ContractListDto>> GetContractFilter(ContractFilterDto contractFilterDto, CancellationToken cancellation = default)
        {
            (IEnumerable<RentalContract> contracts, int totalCount) = await rentalContractRepository.GetFilterData(contractFilterDto.Search, contractFilterDto.PageNumber, contractFilterDto.PageSize, contractFilterDto.FromDate, contractFilterDto.ToDate, contractFilterDto.Status, cancellation);
            return new PaginatedDataDto<ContractListDto>
                (mapper.Map<IEnumerable<ContractListDto>>(contracts), totalCount);
        }
        private decimal CalculateLateReturnFee(DateTime actualReturnDate, DateTime expectedReturnDate, decimal dateReturnFeeMultiplier, decimal HourlyRate)
        {
            return ((decimal)(Math.Ceiling((actualReturnDate - expectedReturnDate).TotalHours))) * HourlyRate * dateReturnFeeMultiplier;
        }
        private ContractDto MapToContractDto(RentalContract contract, Motorbike motorbike, Customer customer, Employee employee)
        {
            ContractDto contractDto = mapper.Map<ContractDto>(contract);
            contractDto.EmployeeFullName = employee.FullName;
            contractDto.CustomerFullName = customer.FullName;
            contractDto.MotorbikeName = motorbike.MotorbikeName;
            contractDto.MotorbikeImageUrl = motorbike.ImageUrl;
            contractDto.MotorbikeLicensePlate = motorbike.LicensePlate;
            contractDto.CategoryName = motorbike.Category.CategoryName;
            return contractDto;
        }
        private RentalPriceResponseDto MapToRentalPriceResponseDto(RentalPriceRequestDto rental, Motorbike motorbike, Discount? discount)
        {
            (decimal basePrice, decimal? discountAmount) = GetPriceWithDiscount(rental.RentalDate,
                rental.ExpectedReturnDate,
                rental.RentalTypeStatus,
                rental.RentalTypeStatus == RentalTypeStatus.Hourly ? motorbike.PriceList.HourlyRate : motorbike.PriceList.DailyRate,
                discount != null ? discount.Value : null);
            return new RentalPriceResponseDto
            {
                MotorbikeId = motorbike.MotorbikeId,
                RentalDate = rental.RentalDate,
                ExpectedReturnDate = rental.ExpectedReturnDate,
                RentalTypeStatus = rental.RentalTypeStatus,
                DepositAmount = motorbike.Category.DepositAmount,
                DiscountName = discount?.Name,
                DiscountValue = discount?.Value,
                RentalPrice = basePrice,
                DiscountAmount = discountAmount,
                TotalPrice = discountAmount != null ? basePrice - discountAmount.Value : basePrice
            };
        }
        private (decimal, decimal?) GetPriceWithDiscount(DateTime rentalDate, DateTime expectedReturnDate, RentalTypeStatus rentalTypeStatus, decimal priceMotorbike, int? discountPercent)
        {
            decimal basePrice = GetBaseRentalPrice(rentalDate, expectedReturnDate, rentalTypeStatus, priceMotorbike);
            decimal? discountAmount = discountPercent != null ? basePrice * discountPercent / 100 : null;
            return (basePrice, discountAmount);
        }
        private decimal GetBaseRentalPrice(DateTime rentalDate, DateTime expectedReturnDate, RentalTypeStatus rentalTypeStatus, decimal priceMotorbike)
        {
            if (rentalTypeStatus == RentalTypeStatus.Hourly) return ((decimal)(Math.Ceiling((expectedReturnDate - rentalDate).TotalHours))) * priceMotorbike;
            if (rentalTypeStatus == RentalTypeStatus.Daily) return ((decimal)(Math.Ceiling((expectedReturnDate - rentalDate).TotalDays))) * priceMotorbike;
            return 0;
        }
    }
}
