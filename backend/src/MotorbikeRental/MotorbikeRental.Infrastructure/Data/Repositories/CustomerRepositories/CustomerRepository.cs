using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using MotorbikeRental.Application.Exceptions;

namespace MotorbikeRental.Infrastructure.Data.Repositories.CustomerRepositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Customer> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Customers.AsNoTracking()
                .Where(c => c.CustomerId == id)
                .Include(c => c.RentalContracts)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Customer not found");
        }
        public async Task<(IEnumerable<Customer>, int totalCount)> GetFilterData(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            IQueryable<Customer> queryable = dbContext.Customers
                .AsNoTracking()
                .Include(c => c.RentalContracts)
                .OrderByDescending(c => c.CustomerId)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                queryable = queryable.Where(e => e.FullName.Contains(lowerSearch) || e.IdNumber.Contains(lowerSearch) || e.PhoneNumber.Contains(lowerSearch) || e.Email.Contains(lowerSearch));
            }
            int totalCount = await queryable.CountAsync();
            queryable = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return (await queryable.ToListAsync(cancellationToken), totalCount);
        }
        public async Task<Customer?> GetCustomerBasicInfoById(int customerId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Customers.AsNoTracking()
                .Where(c => c.CustomerId == customerId)
                .Select(c => new Customer
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}