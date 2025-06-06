using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.DTOs.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;

namespace MotorbikeRental.Application.Validators.VehicleValidators
{
    public class CategoryValidator : ICategoryValidator
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryValidator(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public async Task<bool> ValidateForCreate(CategoryDto categoryViewModel)
        {
            if (await categoryRepository.CategoryNameExists(categoryViewModel.CategoryName))
                throw new Exception("Category name already exists");
            return true;
        }

        public async Task<bool> ValidateForDelete(int id)
        {
            if (!await categoryRepository.CategoryIdExists(id))
                throw new Exception("Category not found");
            return true;
        }

        public async Task<bool> ValidateForGet(int id)
        {
            if (!await categoryRepository.CategoryIdExists(id))
                throw new Exception("Category ID not found");
            return true;
        }

        public async Task<bool> ValidateForUpdate(CategoryDto categoryDto)
        {
            if (!await categoryRepository.CategoryIdExists(categoryDto.CategoryId))
                throw new Exception($"Entity with ID: {categoryDto.CategoryId} not found");
            return true;
        }
    }
}