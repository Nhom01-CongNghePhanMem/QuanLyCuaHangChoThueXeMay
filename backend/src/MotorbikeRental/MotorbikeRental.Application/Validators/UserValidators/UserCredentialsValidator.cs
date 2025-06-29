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
        private readonly IEmployeeRepository employeeRepository;
        public UserCredentialsValidator(IUserCredentialsRepository userCredentialsRepository, UserManager<UserCredentials> userManager, RoleManager<Roles> roleManager, IEmployeeRepository employeeRepository)
        {
            this.userCredentialsRepository = userCredentialsRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.employeeRepository = employeeRepository;
        }
        public async Task<bool> ValidatorForCreate(UserCredentialsCreateDto userCredentialsCreateDto, CancellationToken cancellationToken = default)
        {
            if (!await employeeRepository.IsExists(nameof(Employee.EmployeeId), userCredentialsCreateDto.EmployeeId, cancellationToken))
                throw new NotFoundException($"Employee with id {userCredentialsCreateDto.EmployeeId} not found");
            return true;
        }
        public async Task<bool> ValidatorForUpdate(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default)
        {
            return true;
        }
    }
}