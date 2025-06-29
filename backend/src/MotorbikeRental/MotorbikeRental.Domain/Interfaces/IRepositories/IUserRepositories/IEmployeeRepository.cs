using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Enums.UserEnum;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<Employee> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default);
        Task<(IEnumerable<Employee>, int totalCount)> GetFilterData(string? search, int pageNumber, int pageSize, int? RoleId, EmployeeStatus? status, CancellationToken cancellation = default);
    }
}