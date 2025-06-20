using AutoMapper;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;

namespace MotorbikeRental.Application.Services.VehicleServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper mapper;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICategoryValidator categoryValidator;
        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository, ICategoryValidator categoryValidator)
        {
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
            this.categoryValidator = categoryValidator;
        }
        public async Task<Category> CreateCategory(CategoryDto categoryDto, CancellationToken cancellationToken = default)
        {
            await categoryValidator.ValidateForCreate(categoryDto, cancellationToken);
            Category category = mapper.Map<Category>(categoryDto);
            await categoryRepository.Create(category, cancellationToken);
            return category;
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategories(CancellationToken cancellationToken = default)
        {
            IEnumerable<Category> categories = await categoryRepository.GetAll(cancellationToken);
            return mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
        public async Task<bool> DeleteCategory(int id, CancellationToken cancellationToken = default)
        {
            if (id < 0)
                throw new ValidatorException("Invalid category ID");
            await categoryValidator.ValidateForDelete(id, cancellationToken);
            await categoryRepository.Delete(await categoryRepository.GetById(id, cancellationToken), cancellationToken);
            return true;
        }
        public async Task<CategoryDto> UpdateCategory(CategoryDto categoryDto, CancellationToken cancellationToken = default)
        {
            await categoryValidator.ValidateForUpdate(categoryDto, cancellationToken);
            Category category = mapper.Map<Category>(categoryDto);
            await categoryRepository.Update(category, cancellationToken);
            return categoryDto;
        }
        public async Task<CategoryDto> GetCategoryById(int id, CancellationToken cancellationToken = default)
        {
            if (id < 0)
                throw new ValidatorException("Invalid category ID");
            await categoryValidator.ValidateForGet(id, cancellationToken);
            Category category = await categoryRepository.GetById(id, cancellationToken);
            return mapper.Map<CategoryDto>(category);
        }
    }
}