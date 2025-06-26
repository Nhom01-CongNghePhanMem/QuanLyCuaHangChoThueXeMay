using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;

namespace MotorbikeRental.Application.Validators.UserValidators
{
    public class UserCredentialsValidator : IUserCredentialsValidator
    {
        private readonly IUserCredentialsRepository userCredentialsRepository;
        private readonly UserManager<UserCredentials> userManager;
        private readonly RoleManager<Roles> roleManager;
        public UserCredentialsValidator(IUserCredentialsRepository userCredentialsRepository, UserManager<UserCredentials> userManager, RoleManager<Roles> roleManager)
        {
            this.userCredentialsRepository = userCredentialsRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<bool> ValidatorForCreate(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default)
        {
            var errors = new List<string>();
            if (await userManager.FindByEmailAsync(employeeCreateDto.Email) != null)
                errors.Add($"Employee with email {employeeCreateDto.Email} already exists");
            if (await userCredentialsRepository.IsExists(nameof(UserCredentials.PhoneNumber), employeeCreateDto.PhoneNumber, cancellationToken))
                errors.Add($"Employee with phone number {employeeCreateDto.PhoneNumber} already exists");
            if (!string.IsNullOrEmpty(employeeCreateDto.UserName) && await userManager.FindByNameAsync(employeeCreateDto.UserName) != null)
                errors.Add($"Employee with UserName {employeeCreateDto.UserName} already exists");
            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));
            return true;
        }
        public async Task<bool> ValidatorForUpdate(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default)
        {
            var errors = new List<string>();
            if (await roleManager.FindByIdAsync(employeeUpdateDto.RoleId.ToString()) == null)
                throw new NotFoundException($"Role with id {employeeUpdateDto.RoleId} not found");
            if (await userCredentialsRepository.IsExistsForUpdate(nameof(UserCredentials.Email), employeeUpdateDto.Email, typeof(Employee).Name, nameof(Employee.EmployeeId), employeeUpdateDto.EmployeeId, cancellationToken))
                errors.Add($"Employee with Email {employeeUpdateDto.Email} already exists");
            if (await userCredentialsRepository.IsExistsForUpdate(nameof(UserCredentials.PhoneNumber), employeeUpdateDto.PhoneNumber, typeof(Employee).Name, nameof(Employee.EmployeeId), employeeUpdateDto.EmployeeId, cancellationToken))
                errors.Add($"Employee with PhoneNumber {employeeUpdateDto.PhoneNumber} already exists");
            if (await userCredentialsRepository.IsExistsForUpdate(nameof(UserCredentials.UserName), employeeUpdateDto.UserName, typeof(Employee).Name, nameof(Employee.EmployeeId), employeeUpdateDto.EmployeeId, cancellationToken))
                errors.Add($"Employee with UserName {employeeUpdateDto.UserName} already exists");
            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));
            return true;
        }

    }
}