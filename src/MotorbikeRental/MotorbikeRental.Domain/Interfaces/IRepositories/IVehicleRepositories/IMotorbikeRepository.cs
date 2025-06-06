using MotorbikeRental.Domain.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories
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
        Task<(IEnumerable<Motorbike>, int totalCount)> GetFilterData(MotorbikeFilterDto motorbikeFilterDto);
        Task<IEnumerable<string>> GetDistinctBrands();
    }
}