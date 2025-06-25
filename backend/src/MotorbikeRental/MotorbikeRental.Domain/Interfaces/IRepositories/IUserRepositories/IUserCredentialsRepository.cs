using MotorbikeRental.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories
{
    public interface IUserCredentialsRepository : IBaseRepository<UserCredentials>
    {
        Task<bool> IsExistsForUpdate<Tvalue, Tid>(string key, Tvalue value, string entity, string idPropertyName, Tid id, CancellationToken cancellationToken = default);
        Task<UserCredentials> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default);       
    }
}
