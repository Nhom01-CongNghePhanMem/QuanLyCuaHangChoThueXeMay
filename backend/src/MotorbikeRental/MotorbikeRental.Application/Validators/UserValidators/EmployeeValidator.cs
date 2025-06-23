using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;

namespace MotorbikeRental.Application.Validators.UserValidators
{
    public class EmployeeValidator : IEmployeeValidator
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IRoleRepository roleRepository;
        public EmployeeValidator(IEmployeeRepository employeeRepository, IRoleRepository roleRepository)
        {
            this.employeeRepository = employeeRepository;
            this.roleRepository = roleRepository;
        }
        public async Task<bool> ValidatorForCreate(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default)
        {
            if (!await roleRepository.IsExists(nameof(Roles.RoleId), employeeCreateDto.RoleId, cancellationToken))
                throw new NotFoundException("Role id not found");
            if (await employeeRepository.IsExists(nameof(Employee.Email), employeeCreateDto.Email, cancellationToken))
                throw new ValidatorException("Email already exists");
            if (await employeeRepository.IsExists(nameof(Employee.PhoneNumber), employeeCreateDto.PhoneNumber, cancellationToken))
                throw new ValidatorException("Phone number already exists");
            return true;
        }

        public async Task<bool> ValidatorForUpdate(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default)
        {
            if (!await employeeRepository.IsExists(nameof(Employee.RoleId), employeeUpdateDto.RoleId, cancellationToken))
                throw new NotFoundException("Role id not found");
            if (!await employeeRepository.IsExistsForUpdate(employeeUpdateDto.UserId, nameof(Employee.Email), employeeUpdateDto.Email, nameof(Employee.UserId), cancellationToken))
                throw new ValidatorException("Email already exists");
            if (!await employeeRepository.IsExistsForUpdate(employeeUpdateDto.UserId, nameof(Employee.PhoneNumber), employeeUpdateDto.PhoneNumber, nameof(Employee.UserId), cancellationToken))
                throw new ValidatorException("Phone number already exists");
            return true;
        }
    }
}