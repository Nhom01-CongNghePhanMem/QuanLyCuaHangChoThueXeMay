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
        private readonly IMotorbikeRepository motorbikeRepository;
        public CategoryValidator(ICategoryRepository categoryRepository, IMotorbikeRepository motorbikeRepository)
        {
            this.categoryRepository = categoryRepository;
            this.motorbikeRepository = motorbikeRepository;
        }
        public async Task<bool> ValidateForCreate(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default)
        {
            if (await categoryRepository.IsExists(nameof(Category.CategoryName), categoryCreateDto.CategoryName, cancellationToken))
                throw new ValidatorException("Category name already exists");
            return true;
        }

        public async Task<bool> ValidateForDelete(Category category, CancellationToken cancellationToken = default)
        {
            if(await motorbikeRepository.IsExists(nameof(Motorbike.CategoryId), category.CategoryId, cancellationToken))
                throw new ValidatorException("Cannot delete category because it is associated with existing motorbikes");
            return true;
        }

        public async Task<bool> ValidateForGet(int id, CancellationToken cancellationToken = default)
        {
            if (!await categoryRepository.IsExists(nameof(Category.CategoryId), id, cancellationToken))
                throw new ValidatorException("Category ID not found");
            return true;
        }

        public async Task<bool> ValidateForUpdate(CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default)
        {
            if (!await categoryRepository.IsExists(nameof(Category.CategoryId), categoryUpdateDto.CategoryId, cancellationToken))
                throw new ValidatorException($"Entity with ID: {categoryUpdateDto.CategoryId} not found");
            if (await categoryRepository.IsExistsForUpdate(categoryUpdateDto.CategoryId, nameof(Category.CategoryName), categoryUpdateDto.CategoryName, nameof(Category.CategoryId)))
                throw new ValidatorException($"Category name already exists");
            return true;
        }
    }
}