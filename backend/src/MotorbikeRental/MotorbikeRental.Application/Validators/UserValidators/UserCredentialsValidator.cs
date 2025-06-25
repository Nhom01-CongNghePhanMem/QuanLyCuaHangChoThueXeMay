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
        public async Task<IdentityResult> ValidatorForCreate(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default)
        {
            List<IdentityError> errors = new List<IdentityError>();
            if (await roleManager.FindByIdAsync(employeeCreateDto.RoleId.ToString()) == null)
                errors.Add(new IdentityError { Code = "RoleNotFound", Description = $"Role with id {employeeCreateDto.RoleId} not found" });
            if (await userManager.FindByEmailAsync(employeeCreateDto.Email) != null)
                errors.Add(new IdentityError { Code = "EmailAlreadyExists", Description = $"Employee with email {employeeCreateDto.Email} already exists" });
            if (await userCredentialsRepository.IsExists(nameof(UserCredentials.PhoneNumber), employeeCreateDto.PhoneNumber, cancellationToken))
                errors.Add(new IdentityError { Code = "PhoneNumberAlreadyExists", Description = $"Employee with phone number {employeeCreateDto.PhoneNumber} already exists" });
            if (await userManager.FindByNameAsync(employeeCreateDto.UserName) != null)
                errors.Add(new IdentityError { Code = "UserNameAlreadyExists", Description = $"Employee with UserName {employeeCreateDto.UserName} already exists" });
            return errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
        public async Task<IdentityResult> ValidatorForUpdate(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default)
        {
            List<IdentityError> errors = new List<IdentityError>();
            if(await roleManager.FindByIdAsync(employeeUpdateDto.RoleId.ToString()) == null)
                errors.Add(new IdentityError { Code = "RoleNotFound", Description = $"Role with id {employeeUpdateDto.RoleId} not found" });
            if (await userCredentialsRepository.IsExistsForUpdate(nameof(UserCredentials.Email), employeeUpdateDto.Email, typeof(Employee).Name, nameof(Employee.EmployeeId), employeeUpdateDto.EmployeeId, cancellationToken))
                errors.Add(new IdentityError { Code = "EmailAlreadyExists", Description = $"Employee with Email {employeeUpdateDto.Email} already exists" });
            if (await userCredentialsRepository.IsExistsForUpdate(nameof(UserCredentials.PhoneNumber), employeeUpdateDto.PhoneNumber, typeof(Employee).Name, nameof(Employee.EmployeeId), employeeUpdateDto.EmployeeId, cancellationToken))
                errors.Add(new IdentityError { Code = "PhoneNumberAlreadyExists", Description = $"Employee with Email {employeeUpdateDto.PhoneNumber} already exists" });
            if (await userCredentialsRepository.IsExistsForUpdate(nameof(UserCredentials.UserName), employeeUpdateDto.UserName, typeof(Employee).Name, nameof(Employee.EmployeeId), employeeUpdateDto.EmployeeId, cancellationToken))
                errors.Add(new IdentityError { Code = "UserNameAlreadyExists", Description = $"Employee with UserName {employeeUpdateDto.UserName} already exists" });
            return errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }

    }
}