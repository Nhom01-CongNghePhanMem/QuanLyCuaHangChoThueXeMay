using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Application.Interface.IServices.IUserServices
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRoles(CancellationToken cancellationToken = default);
        Task<Roles> GetRoleById(int id, CancellationToken cancellationToken = default);
    }
}