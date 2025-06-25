using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.UserRepositories
{
    public class UserCredentialsRepository : BaseRepository<UserCredentials>, IUserCredentialsRepository
    {
        public UserCredentialsRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext)
        {
        }
    }
}
