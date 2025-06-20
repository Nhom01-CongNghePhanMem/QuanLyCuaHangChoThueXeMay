using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Application.Interface.IServices.IVehicleServices
{
    public interface ICategoryService
    {
        Task<Category> CreateCategory(CategoryDto categoryDto, CancellationToken cancellationToken = default);
        Task<IEnumerable<CategoryDto>> GetAllCategories(CancellationToken cancellationToken = default);
        Task<bool> DeleteCategory(int id, CancellationToken cancellationToken = default);
        Task<CategoryDto> UpdateCategory(CategoryDto categoryDto, CancellationToken cancellationToken = default);
        Task<CategoryDto> GetCategoryById(int id, CancellationToken cancellationToken = default);
    }
}