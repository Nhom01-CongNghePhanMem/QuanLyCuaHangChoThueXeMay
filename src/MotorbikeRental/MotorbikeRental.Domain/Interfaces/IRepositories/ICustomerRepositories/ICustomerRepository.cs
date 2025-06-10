using MotorbikeRental.Domain.Entities.Customers;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<Customer> GetByIdWithIncludes(int id);
    }
}