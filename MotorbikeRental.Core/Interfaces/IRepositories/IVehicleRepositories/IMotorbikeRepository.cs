using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General;

namespace MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories
{
    public interface IMotorbikeRepository : IBaseRepository<Motorbike>
    {
        Task<bool> IsExists(int id);
        Task<bool> LicensePlateExists(string LicensePlate);
        Task<bool> ChassisNumberExists(string ChassisNumber);
        Task<bool> EngineNumberExists(string EngineNumber);
    }
}