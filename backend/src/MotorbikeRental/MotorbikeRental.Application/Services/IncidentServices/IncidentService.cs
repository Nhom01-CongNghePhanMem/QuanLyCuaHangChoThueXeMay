using MotorbikeRental.Application.DTOs.Incident;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IServices.IIncidentServices;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IIncidents;
using MotorbikeRental.Domain.Enums.ContractEnum;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using AutoMapper;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;
using MotorbikeRental.Application.Validators.IncidentValidators;
using MotorbikeRental.Application.Interface.IValidators.IIncidentValidators;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Application.Interface.IExternalServices.Storage;
using System.Threading.Tasks;
using System.Security.Principal;

namespace MotorbikeRental.Application.Services.IncidentServices
{
    public class IncidentService : IIncidentService
    {
        private readonly IRentalContractRepository rentalContractRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMotorbikeRepository motorbikeRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;
        private readonly IIncidentRepository incidentRepository;
        private readonly IIncidentValidator incidentValidator;
        private readonly IFileService fileService;
        private readonly IIncidentImageRepository incidentImageRepository;
        public IncidentService(IRentalContractRepository rentalContractRepository, IIncidentImageRepository incidentImageRepository, IFileService fileService, IMotorbikeRepository motorbikeRepository, IEmployeeRepository employeeRepository, IMapper mapper, IIncidentRepository incidentRepository, ICustomerRepository customerRepository, IIncidentValidator incidentValidator)
        {
            this.rentalContractRepository = rentalContractRepository;
            this.employeeRepository = employeeRepository;
            this.mapper = mapper;
            this.motorbikeRepository = motorbikeRepository;
            this.incidentRepository = incidentRepository;
            this.customerRepository = customerRepository;
            this.incidentValidator = incidentValidator;
            this.fileService = fileService;
            this.incidentImageRepository = incidentImageRepository;
        }
        public async Task<IncidentDto> CreateIncident(IncidentCreateDto incidentCreateDto, CancellationToken cancellationToken = default)
        {
            RentalContract rentalContract = await rentalContractRepository.GetContractById(incidentCreateDto.ContractId, cancellationToken) ?? throw new NotFoundException("Rental contract not found");
            Employee employee = await employeeRepository.GetEmployeeBasicInfoById(incidentCreateDto.ReportedByEmployeeId.Value, cancellationToken) ?? throw new NotFoundException("Employee not found");
            Motorbike motorbike = await motorbikeRepository.GetByIdWithIncludes(rentalContract.MotorbikeId.Value, cancellationToken) ?? throw new NotFoundException("Motorbike not found");
            Customer? customer = await customerRepository.GetCustomerBasicInfoById(rentalContract.CustomerId.Value, cancellationToken);
            await incidentValidator.ValidateForCreate(incidentCreateDto, rentalContract, cancellationToken);
            Incident incident = mapper.Map<Incident>(incidentCreateDto);
            incident.MotorbikeId = motorbike.MotorbikeId;
            incident.CreatedAt = DateTime.UtcNow;
            if (incidentCreateDto.Images != null)
            {
                List<string> images = await fileService.SaveImages(incidentCreateDto.Images, "Incident", cancellationToken);
                incident.Images = images.Select(path => new IncidentImage
                {
                    ImageUrl = path
                }).ToList();
            }
            IncidentDto incidentDto = mapper.Map<IncidentDto>(await incidentRepository.Create(incident, cancellationToken));
            MapToIncidentDto(incidentDto, customer, employee, motorbike);
            return incidentDto;
        }
        public async Task<IncidentDto> UpdateBeforeComplete(IncidentUpdateBeforeCompleteDto incidentUpdateBeforeCompleteDto, CancellationToken cancellationToken = default)
        {
            Incident incident = await incidentRepository.GetIncidentByIdWithIncludes(incidentUpdateBeforeCompleteDto.IncidentId, true, cancellationToken) ?? throw new NotFoundException("Incident not found");
            RentalContract rentalContract = await rentalContractRepository.GetContractById(incidentUpdateBeforeCompleteDto.ContractId, cancellationToken) ?? throw new NotFoundException("Rental contract not found");
            Employee employee = await employeeRepository.GetEmployeeBasicInfoById(incidentUpdateBeforeCompleteDto.ReportedByEmployeeId.Value, cancellationToken) ?? throw new NotFoundException("Employee not found");
            Motorbike motorbike = await motorbikeRepository.GetByIdWithIncludes(rentalContract.MotorbikeId.Value, cancellationToken) ?? throw new NotFoundException("Motorbike not found");
            Customer? customer = await customerRepository.GetCustomerBasicInfoById(rentalContract.CustomerId.Value, cancellationToken);
            await incidentValidator.ValidateForUpdateBeforeActivation(incidentUpdateBeforeCompleteDto, rentalContract, cancellationToken);
            mapper.Map(incidentUpdateBeforeCompleteDto, incident);
            if (incidentUpdateBeforeCompleteDto.Images != null)
            {
                if (incident.Images != null)
                {
                    if (!fileService.DeleteFiles(incident.Images.Select(im => im.ImageUrl).ToList())) throw new Exception("Failed to delete file");
                    incident.Images.Clear();
                    await incidentRepository.SaveChangeAsync(cancellationToken);
                }
                List<string> images = await fileService.SaveImages(incidentUpdateBeforeCompleteDto.Images, "Incident", cancellationToken);
                incident.Images = images.Select(path => new IncidentImage
                {
                    ImageUrl = path
                }).ToList();
            }
            await incidentRepository.Update(incident, cancellationToken);
            incident.MotorbikeId = rentalContract.MotorbikeId;
            IncidentDto incidentDto = mapper.Map<IncidentDto>(incident);
            MapToIncidentDto(incidentDto, customer, employee, motorbike);
            return incidentDto;
        }
        public async Task<IncidentDto> GetincidentById(int id, CancellationToken cancellation = default)
        {
            Incident incident = await incidentRepository.GetIncidentByIdWithIncludes(id, false, cancellation) ?? throw new NotFoundException("Incident not found");
            RentalContract? rentalContract = await rentalContractRepository.GetContractById(incident.ContractId.Value, cancellation);
            Customer? customer = await customerRepository.GetCustomerBasicInfoById(rentalContract.CustomerId.Value, cancellation);
            Employee employee = await employeeRepository.GetEmployeeBasicInfoById(incident.ReportedByEmployeeId.Value, cancellation) ?? throw new NotFoundException("Employee not found");
            Motorbike motorbike = await motorbikeRepository.GetByIdWithIncludes(incident.MotorbikeId.Value, cancellation) ?? throw new NotFoundException("Motorbike not found");
            IncidentDto incidentDto = mapper.Map<IncidentDto>(incident);
            MapToIncidentDto(incidentDto, customer, employee, motorbike);
            return incidentDto;
        }
        public async Task<bool> DeleteIncident(int incidentId, CancellationToken cancellationToken = default)
        {
            Incident? incident = await incidentRepository.GetById(incidentId, cancellationToken) ?? throw new NotFoundException("Incident not found");
            if (incident.Images != null && !fileService.DeleteFiles(incident.Images.Select(im => im.ImageUrl).ToList())) throw new Exception("Failed to delete file");
            await incidentRepository.Delete(incident, cancellationToken);
            return true;
        }
        private void MapToIncidentDto(IncidentDto incidentDto, Customer customer, Employee employee, Motorbike motorbike)
        {
            incidentDto.CustomerName = customer.FullName;
            incidentDto.MotorbikeName = motorbike.MotorbikeName;
            incidentDto.MotorbikeImage = motorbike.ImageUrl;
            incidentDto.ReportedByEmployeeName = employee.FullName;
        }
    }
}
