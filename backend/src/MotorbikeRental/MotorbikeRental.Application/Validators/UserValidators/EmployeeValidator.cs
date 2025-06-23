using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;

namespace MotorbikeRental.Application.Validators.UserValidators
{
    public class EmployeeValidator : IEmployeeValidator
    {
        private readonly IEmployeeRepository employeeRepository;
        public EmployeeValidator(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        public Task<bool> ValidatorForCreate(EmployeeCreateDto employeeCreateDto, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidatorForUpdate(EmployeeUpdateDto employeeUpdateDto, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }
    }
}