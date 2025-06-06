using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<bool> CategoryNameExists(string categoryName);
        Task<bool> CategoryIdExists(int id);
        Task<Category> GetByIdNoAsTracking(int id);
        Task<IEnumerable<Category>> GetCategoriesNoTracking();
    }
}