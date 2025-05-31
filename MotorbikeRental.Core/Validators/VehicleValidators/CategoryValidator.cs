using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Core.Interfaces.IValidators.IVehicleValidators;

namespace MotorbikeRental.Core.Validators.VehicleValidators
{
    public class CategoryValidator : ICategoryValidator
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryValidator(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public async Task<bool> ValidateForCreate(CategoryViewModel categoryViewModel)
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

        public async Task<bool> ValidateForUpdate(CategoryViewModel categoryViewModel)
        {
            if (!await categoryRepository.CategoryIdExists(categoryViewModel.CategoryId))
                throw new Exception($"Entity with ID: {categoryViewModel.CategoryId} not found");
            return true;
        }
    }
}