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
            return await dbContext.Customers.Where(c => c.CustomerId == id).AsNoTracking().Include(c => c.RentalContracts).FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Customer not found");
        }
    }
}