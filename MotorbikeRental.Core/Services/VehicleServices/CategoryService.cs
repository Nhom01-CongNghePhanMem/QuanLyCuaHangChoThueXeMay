using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Core.Interfaces.IServices.IVehicleServices;

namespace MotorbikeRental.Core.Services.VehicleServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper mapper;
        private readonly ICategoryRepository categoryRepository;
        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository)
        {
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }
        public async Task<Category> CreateCategory(CategoryViewModel categoryViewModel)
        {
            bool isExist = await categoryRepository.CategoryNameExists(categoryViewModel.CategoryName);
            if (!isExist)
            {
                Category category = mapper.Map<Category>(categoryViewModel);
                await categoryRepository.Create(category);
                return category;
            }
            throw new Exception("Category name already exists");
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
            bool exists = await categoryRepository.CategoryIdExists(id);
            if (!exists)
                throw new Exception("Category not found");
            Category category = await categoryRepository.GetById(id);
            await categoryRepository.Delete(category);
            return true;
        }
        public async Task<CategoryViewModel> UpdateCategory(CategoryViewModel categoryViewModel)
        {
            bool exists = await categoryRepository.CategoryIdExists(categoryViewModel.CategoryId);
            if (!exists)
                throw new Exception($"Entity with ID: {categoryViewModel.CategoryId} not found");
            Category category = mapper.Map<Category>(categoryViewModel);
            await categoryRepository.Update(category);
            return categoryViewModel;
        }
        public async Task<CategoryViewModel> GetCategoryById(int id)
        {
            if (id < 0)
                throw new Exception("Invalid category ID");
            bool exists = await categoryRepository.CategoryIdExists(id);
            if (!exists)
                throw new Exception("Category ID not found");
            Category category = await categoryRepository.GetById(id);
            return mapper.Map<CategoryViewModel>(category);
        }
    }
}