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
            if (await roleManager.FindByIdAsync(employeeCreateDto.RoleId.ToString()) == null)
                return IdentityResult.Failed(new IdentityError { Code = "RoleNotFound", Description = $"Role with id {employeeCreateDto.RoleId} not found" });
            if (await userManager.FindByEmailAsync(employeeCreateDto.Email) == null)
                return IdentityResult.Failed(new IdentityError { Code = "Email already exists", Description = "" });
            return null;
        }

        Task<bool> IUserCredentialsValidator.ValidatorForCreate(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}