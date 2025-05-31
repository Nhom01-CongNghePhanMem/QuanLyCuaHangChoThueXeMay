using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General;

namespace MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<bool> CategoryNameExists(string categoryName);
        Task<bool> CategoryIdExists(int id);
        Task<Category> GetByIdNoAsTracking(int id);
    }
}