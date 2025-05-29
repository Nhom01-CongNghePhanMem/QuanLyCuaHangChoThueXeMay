using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.Business.User;
using MotorbikeRental.Core.Entities.General.User;

namespace MotorbikeRental.Core.Interfaces.IServices.IUserServices
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleViewModel>> GetAllRoles();
        Task<Roles> GetRoleById(int id);
    }
}