using AutoMapper;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IExternalServices.ISecurityServices;
using MotorbikeRental.Application.Interface.IExternalServices.Storage;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;
using MotorbikeRental.Application.Validators.UserValidators;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;

namespace MotorbikeRental.Application.Services.UserServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper mapper;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IEmployeeValidator employeeValidator;
        private readonly IPasswordHasher passwordHasher;
        private readonly IFileService fileService;
        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository, IEmployeeValidator employeeValidator, IPasswordHasher passwordHasher, IFileService fileService)
        {
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
            this.employeeValidator = employeeValidator;
            this.passwordHasher = passwordHasher;
            this.fileService = fileService;
        }

        public async Task<EmployeeDto> CreateEmployee(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default)
        {
            await employeeValidator.ValidatorForCreate(employeeCreateDto, cancellationToken);
            Employee employee = mapper.Map<Employee>(employeeCreateDto);
            employee.UserCredentials = new UserCredentials
            {
                Username = employeeCreateDto.Username,
                PasswordHash = passwordHasher.HashPassword(employeeCreateDto.Password)
            };
            if (employeeCreateDto.FormFile != null)
            {
                employee.Avatar = await fileService.SaveImage(employeeCreateDto.FormFile, "Employee", cancellationToken);
            }
            return mapper.Map<EmployeeDto>(await employeeRepository.Create(employee, cancellationToken));
        }

        public async Task<bool> DeleteEmployee(int id, CancellationToken cancellationToken = default)
        {
            Employee employee = await employeeRepository.GetById(id, cancellationToken) ?? throw new NotFoundException("Employee not found");
            await employeeRepository.Delete(employee, cancellationToken);
            if (employee.Avatar != null)
                fileService.DeleteFile(employee.Avatar);
            return true;
        }

        public async Task<EmployeeDto> GetEmployeeById(int id, CancellationToken cancellationToken = default)
        {
            return mapper.Map<EmployeeDto>(await employeeRepository.GetByIdWithIncludes(id, cancellationToken));

        }

        public Task<EmployeeDto> UpdateEmployee(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}