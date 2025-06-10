using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories
{
    public interface IMotorbikeRepository : IBaseRepository<Motorbike>
    {
        Task<Motorbike> GetByIdNoAsTracking(int id);
        Task<Motorbike> GetByIdWithIncludes(int id);
        Task<(IEnumerable<Motorbike>, int totalCount)> GetFilterData(int? categoryId, string? brand, MotorbikeStatus? status, int pageNumber, int pageSize);
        Task<IEnumerable<string>> GetDistinctBrands();
    }
}