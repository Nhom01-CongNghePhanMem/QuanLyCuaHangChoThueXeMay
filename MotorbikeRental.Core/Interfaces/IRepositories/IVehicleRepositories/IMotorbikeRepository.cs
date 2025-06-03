using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.Business.Pagination;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Enums;

namespace MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories
{
    public interface IMotorbikeRepository : IBaseRepository<Motorbike>
    {
        Task<bool> IsExists(int id);
        Task<bool> LicensePlateExists(string licensePlate);
        Task<bool> ChassisNumberExists(string chassisNumber);
        Task<bool> EngineNumberExists(string engineNumber);
        Task<Motorbike> GetByIdNoAsTracking(int id);
        Task<bool> DupChassisNumExceptId(string chassisNumber, int id);
        Task<bool> DupEngineNumExceptId(string engineNumber, int id);
        Task<bool> DupLicensePlateExceptId(string licensePlate, int id);
        Task<Motorbike> GetByIdWithIncludes(int id);
        Task<(IEnumerable<Motorbike>, int totalCount)> GetFilterData(MotorbikeFilterViewModel motorbikeFilterViewModel);
        Task<IEnumerable<string>> GetDistinctBrands();
    }
}