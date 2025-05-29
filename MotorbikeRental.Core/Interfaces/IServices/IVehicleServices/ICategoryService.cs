using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;

namespace MotorbikeRental.Core.Interfaces.IServices.IVehicleServices
{
    public interface ICategoryService
    {
        Task<Category> CreateCategory(CategoryViewModel categoryViewModel);
        Task<IEnumerable<CategoryViewModel>> GetAllCategories();
        Task<bool> DeleteCategory(int id);
        Task<CategoryViewModel> UpdateCategory(CategoryViewModel categoryViewModel);
        Task<CategoryViewModel> GetCategoryById(int id);
    }
}