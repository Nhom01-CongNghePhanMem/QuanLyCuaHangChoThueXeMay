using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.User;
using MotorbikeRental.Core.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Infrastructure.Data;

namespace MotorbikeRental.Infrastructure.Repositories.UserRepositories
{
    public class RoleRepository : BaseRepository<Roles> , IRoleRepository
    {
        public RoleRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext){}
    }
}