using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.UserRepositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Employee> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Set<Employee>().Where(e => e.EmployeeId == id)
                .AsNoTracking()
                .Include(e => e.UserCredentials)
                .Include(e => e.UserCredentials.Roles)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Employee not found");
        }
    }
}