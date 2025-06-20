using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Application.Exceptions;
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
        public async Task<bool> ValidateForCreate(CategoryDto categoryViewModel, CancellationToken cancellationToken = default)
        {
            if (await categoryRepository.IsExists(nameof(Category.CategoryName), categoryViewModel.CategoryName, cancellationToken))
                throw new ValidatorException("Category name already exists");
            return true;
        }

        public async Task<bool> ValidateForDelete(int id, CancellationToken cancellationToken = default)
        {
            if (!await categoryRepository.IsExists(nameof(Category.CategoryId), id, cancellationToken))
                throw new ValidatorException("Category not found");
            return true;
        }

        public async Task<bool> ValidateForGet(int id, CancellationToken cancellationToken = default)
        {
            if (!await categoryRepository.IsExists(nameof(Category.CategoryId), id, cancellationToken))
                throw new ValidatorException("Category ID not found");
            return true;
        }

        public async Task<bool> ValidateForUpdate(CategoryDto categoryDto, CancellationToken cancellationToken = default)
        {
            if (!await categoryRepository.IsExists(nameof(Category.CategoryId),categoryDto.CategoryId, cancellationToken))
                throw new ValidatorException($"Entity with ID: {categoryDto.CategoryId} not found");
            return true;
        }
    }
}