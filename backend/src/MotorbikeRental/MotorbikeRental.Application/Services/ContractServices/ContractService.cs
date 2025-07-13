using MotorbikeRental.Application.DTOs.ContractDto;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IServices.IContractServices;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.ContractEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;

namespace MotorbikeRental.Application.Services.ContractServices
{
    public class ContractService : IContractService
    {
        private readonly IMotorbikeRepository motorbikeRepository;
        public ContractService(IMotorbikeRepository motorbikeRepository)
        {
            this.motorbikeRepository = motorbikeRepository;
        }
        public async Task<RentalPriceResponseDto> CalculateRentalPrice(RentalPriceRequestDto priceRequestDto, CancellationToken cancellationToken = default)
        {
            Motorbike motorbike = await motorbikeRepository.GetByIdWithIncludes(priceRequestDto.MotorbikeId, cancellationToken);
            RentalPriceResponseDto rentalPriceResponseDto = new RentalPriceResponseDto
            {
                MotorbikeId = priceRequestDto.MotorbikeId,
                RentalDate = priceRequestDto.RentalDate,
                ExpectedReturnDate = priceRequestDto.ExpectedReturnDate,
                RentalTypeStatus = priceRequestDto.RentalTypeStatus,
                DepositAmount = motorbike.Category.DepositAmount
            };
            if (priceRequestDto.ExpectedReturnDate < priceRequestDto.RentalDate)
                throw new BusinessRuleException("Return date not greater than rental date");
            if (priceRequestDto.RentalTypeStatus == RentalTypeStatus.Hourly)
                rentalPriceResponseDto.RentalPrice = ((decimal)(Math.Ceiling((priceRequestDto.ExpectedReturnDate - priceRequestDto.RentalDate).TotalHours))) * motorbike.PriceList.HourlyRate;
            if(priceRequestDto.RentalTypeStatus == RentalTypeStatus.Daily)
                rentalPriceResponseDto.RentalPrice = ((decimal)(Math.Ceiling((priceRequestDto.ExpectedReturnDate - priceRequestDto.RentalDate).TotalDays))) * motorbike.PriceList.DailyRate;
            return rentalPriceResponseDto;
        }
    }
}
