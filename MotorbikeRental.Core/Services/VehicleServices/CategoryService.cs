using AutoMapper;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Core.Interfaces.IServices.IVehicleServices;
using MotorbikeRental.Core.Interfaces.IValidators.IVehicleValidators;

namespace MotorbikeRental.Core.Services.VehicleServices
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
        public async Task<Category> CreateCategory(CategoryViewModel categoryViewModel)
        {
            await categoryValidator.ValidateForCreate(categoryViewModel);
            Category category = mapper.Map<Category>(categoryViewModel);
            await categoryRepository.Create(category);
            return category;
        }
        public async Task<IEnumerable<CategoryViewModel>> GetAllCategories()
        {
            IEnumerable<Category> categories = await categoryRepository.GetAll();
            return mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        }
        public async Task<bool> DeleteCategory(int id)
        {
            if (id < 0)
                throw new Exception("Invalid category ID");
            await categoryValidator.ValidateForDelete(id);
            Category category = await categoryRepository.GetById(id);
            await categoryRepository.Delete(category);
            return true;
        }
        public async Task<CategoryViewModel> UpdateCategory(CategoryViewModel categoryViewModel)
        {
            await categoryValidator.ValidateForUpdate(categoryViewModel);
            Category category = mapper.Map<Category>(categoryViewModel);
            await categoryRepository.Update(category);
            return categoryViewModel;
        }
        public async Task<CategoryViewModel> GetCategoryById(int id)
        {
            if (id < 0)
                throw new Exception("Invalid category ID");
            await categoryValidator.ValidateForGet(id);
            Category category = await categoryRepository.GetById(id);
            return mapper.Map<CategoryViewModel>(category);
        }
    }
}