using AutoMapper;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.Entities.Vehicles;
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
        public async Task<Category> CreateCategory(CategoryDto categoryDto)
        {
            await categoryValidator.ValidateForCreate(categoryDto);
            Category category = mapper.Map<Category>(categoryDto);
            await categoryRepository.Create(category);
            return category;
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            IEnumerable<Category> categories = await categoryRepository.GetAll();
            return mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
        public async Task<bool> DeleteCategory(int id)
        {
            if (id < 0)
                throw new Exception("Invalid category ID");
            await categoryValidator.ValidateForDelete(id);
            await categoryRepository.Delete(await categoryRepository.GetById(id));
            return true;
        }
        public async Task<CategoryDto> UpdateCategory(CategoryDto categoryDto)
        {
            await categoryValidator.ValidateForUpdate(categoryDto);
            Category category = mapper.Map<Category>(categoryDto);
            await categoryRepository.Update(category);
            return categoryDto;
        }
        public async Task<CategoryDto> GetCategoryById(int id)
        {
            if (id < 0)
                throw new Exception("Invalid category ID");
            await categoryValidator.ValidateForGet(id);
            Category category = await categoryRepository.GetById(id);
            return mapper.Map<CategoryDto>(category);
        }
    }
}