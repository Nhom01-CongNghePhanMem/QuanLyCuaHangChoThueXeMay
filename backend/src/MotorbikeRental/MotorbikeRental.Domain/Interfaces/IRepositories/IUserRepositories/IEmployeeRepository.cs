using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<Employee> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default);
    }
}