using MotorbikeRental.Application.DTOs.Customers;

namespace MotorbikeRental.Application.Interface.IValidators.ICustomerValidators
{
    public interface ICustomerValidator
    {
        Task<bool> ValidateForCreate(CustomerDto customerDto, CancellationToken cancellationToken = default);
        Task<bool> ValidateForUpdate(CustomerDto customerDto, CancellationToken cancellationToken = default);
    }
}