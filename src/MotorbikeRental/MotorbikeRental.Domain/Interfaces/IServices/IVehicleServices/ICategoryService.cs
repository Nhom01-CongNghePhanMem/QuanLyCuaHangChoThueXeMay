using MotorbikeRental.Domain.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Domain.Interfaces.IServices.IVehicleServices
{
    public interface ICategoryService
    {
        Task<Category> CreateCategory(CategoryDto categoryDto);
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<bool> DeleteCategory(int id);
        Task<CategoryDto> UpdateCategory(CategoryDto categoryDto);
        Task<CategoryDto> GetCategoryById(int id);
    }
}