using MotorbikeRental.Application.DTOs.Customers;

namespace MotorbikeRental.Application.Interface.IServices.ICustomerServices
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateCustomer(CustomerDto customerDto);
        Task<CustomerDto> GetCustomerById(int id);
        Task<bool> DeleteCustomer(int id);
    }
}

