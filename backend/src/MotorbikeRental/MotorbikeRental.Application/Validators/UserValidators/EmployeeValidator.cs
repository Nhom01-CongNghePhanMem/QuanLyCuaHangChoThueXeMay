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
        private readonly IUserCredentialsRepository userCredentials;
        public EmployeeValidator(IEmployeeRepository employeeRepository, IRoleRepository roleRepository, IUserCredentialsRepository userCredentials)
        {
            this.employeeRepository = employeeRepository;
            this.roleRepository = roleRepository;
            this.userCredentials = userCredentials;
        }
        public async Task<bool> ValidatorForCreate(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default)
        {
            return true;
        }

        public async Task<bool> ValidatorForUpdate(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default)
        {
            
            return true;
        }
    }
}