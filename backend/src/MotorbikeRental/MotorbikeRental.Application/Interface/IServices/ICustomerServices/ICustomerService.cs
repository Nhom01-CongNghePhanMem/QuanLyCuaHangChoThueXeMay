using MotorbikeRental.Application.DTOs.Customers;

namespace MotorbikeRental.Application.Interface.IServices.ICustomerServices
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateCustomer(CustomerDto customerDto, CancellationToken cancellationToken = default);
        Task<CustomerDto> GetCustomerById(int id, CancellationToken cancellationToken = default);
        Task<bool> DeleteCustomer(int id, CancellationToken cancellationToken = default);
    }
}

