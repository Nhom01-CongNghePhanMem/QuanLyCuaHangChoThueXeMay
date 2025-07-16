using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Enums.UserEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Infrastructure.Data.Repositories.UserRepositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Employee> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Employees.AsNoTracking()
                .Where(e => e.EmployeeId == id)
                .Include(e => e.UserCredentials)
                .ThenInclude(u => u.Roles)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Employee with id{id} not found");
        }
        public async Task<(IEnumerable<Employee>, int totalCount)> GetFilterData(int? employeeId, string? search, int pageNumber, int pageSize, int? RoleId, EmployeeStatus? status, CancellationToken cancellation = default)
        {
            IQueryable<Employee> queryable = dbContext.Employees
                .AsNoTracking()
                .OrderByDescending(e => e.EmployeeId)
                .Include(e => e.UserCredentials)
                .ThenInclude(u => u.Roles)
                .AsQueryable();
            if (employeeId != null)
                queryable = queryable.Where(e => e.EmployeeId != employeeId);
            if (RoleId != null)
                queryable = queryable.Where(e => e.UserCredentials.RoleId == RoleId);
            if (status != null)
                queryable = queryable.Where(e => e.Status == status);
            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                queryable = queryable.Where(e => e.FullName.Contains(lowerSearch) || e.UserCredentials.PhoneNumber.Contains(lowerSearch) || e.UserCredentials.Email.Contains(lowerSearch));
            }
            int totalCount = await queryable.CountAsync();
            queryable = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return (await queryable.ToListAsync(), totalCount);
        }
        public async Task<Employee?> GetEmployeeBasicInfoById(int employeeId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Employees.AsNoTracking()
                .Where(e => e.EmployeeId == employeeId)
                .Select(e => new Employee
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}