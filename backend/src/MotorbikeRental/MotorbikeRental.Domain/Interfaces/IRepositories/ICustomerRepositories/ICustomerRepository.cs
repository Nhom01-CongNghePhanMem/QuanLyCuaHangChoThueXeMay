using MotorbikeRental.Domain.Entities.Customers;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<Customer> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default);
        Task<(IEnumerable<Customer>, int totalCount)> GetFilterData(string? search, int pageNumber, int pageSize, CancellationToken cancellation = default);
    }
}