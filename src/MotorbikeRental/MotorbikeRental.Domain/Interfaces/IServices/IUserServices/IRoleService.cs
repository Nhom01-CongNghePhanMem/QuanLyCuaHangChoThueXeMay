using MotorbikeRental.Domain.DTOs.User;
using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Domain.Interfaces.IServices.IUserServices
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRoles();
        Task<Roles> GetRoleById(int id);
    }
}