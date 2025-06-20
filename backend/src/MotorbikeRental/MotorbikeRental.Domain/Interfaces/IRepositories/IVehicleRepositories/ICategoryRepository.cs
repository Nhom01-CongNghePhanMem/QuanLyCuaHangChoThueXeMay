using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category> GetByIdNoAsTracking(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetCategoriesNoTracking(CancellationToken cancellationToken = default);
    }
}