using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Application.Interface.IValidators.IUserValidators
{
    public interface IEmployeeValidator
    {
        Task<bool> ValidatorForCreate(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default);
        Task<bool> ValidatorForUpdate(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default);
    }
}