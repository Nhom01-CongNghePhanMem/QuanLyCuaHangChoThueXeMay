using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Core.Interfaces.IServices.IVehicleServices;
using MotorbikeRental.Core.Interfaces.IValidators.IVehicleValidators;
using MotorbikeRental.Core.Services.VehicleServices;

namespace MotorbikeRental.UnitTest.Core
{
    public class CategoryServiceTests
    {
        private readonly IMapper mapper;
        private readonly Mock<ICategoryRepository> categoryRepository;
        private readonly Mock<ICategoryValidator> categoryValidator;
        private readonly ICategoryService categoryService;
        public CategoryServiceTests()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryViewModel>();
                cfg.CreateMap<CategoryViewModel, Category>();
            });
            mapper = mapperConfiguration.CreateMapper();
            categoryRepository = new Mock<ICategoryRepository>();
            categoryValidator = new Mock<ICategoryValidator>();
            categoryService = new CategoryService(mapper, categoryRepository.Object, categoryValidator.Object);
        }
        [Fact]
        public async Task CreateCategoryTests()
        {
            CategoryViewModel categoryViewModel = GetCategoryViewModel();
            categoryValidator.Setup(repo => repo.ValidateForCreate(categoryViewModel))
                .ReturnsAsync(true);
            categoryRepository.Setup(r => r.Create(It.IsAny<Category>()));
            Category category = await categoryService.CreateCategory(categoryViewModel);
            Assert.Equal("Sport", category.CategoryName);
        }
        [Fact]
        public void GetAllCategoriesTests()
        {
            List<CategoryViewModel> categoryViewModels = mapper.Map<List<CategoryViewModel>>(GetCategoryViewModels());
            categoryRepository.Setup(repo => repo.GetAll())
                .Returns(() => Task.FromResult<IEnumerable<Category>>(new List<Category>()));
            categoryService.GetAllCategories();
            Assert.Equal(4, categoryViewModels[4].CategoryId);
        }
        public Category GetCategory()
        {
            Category fakeCategory = new Category
            {
                CategoryId = 1,
                CategoryName = "Sport"
            };
            return fakeCategory;
        }
        public CategoryViewModel GetCategoryViewModel()
        {
            var fakeCategory = new CategoryViewModel
            {
                CategoryId = 1,
                CategoryName = "Sport"
            };
            return fakeCategory;
        }
        public List<CategoryViewModel> GetCategoryViewModels()
        {
            List<CategoryViewModel> list = new List<CategoryViewModel>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new CategoryViewModel
                {
                    CategoryId = i,
                    CategoryName = "Xe ga"
                });
            }
            return list;
        }
        public List<Category> GetCategories()
        {
            List<Category> list = new List<Category>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new Category
                {
                    CategoryId = i,
                    CategoryName = "Xe ga"
                });
            }
            return list;
        }
    }
}