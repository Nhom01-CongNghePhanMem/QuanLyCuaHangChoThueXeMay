using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Enums;
using MotorbikeRental.Core.Enums.VehicleEnum;
using MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Core.Interfaces.IServices.IVehicleServices;
using MotorbikeRental.Core.Interfaces.IValidators.IVehicleValidators;
using MotorbikeRental.Core.Services.VehicleServices;

namespace MotorbikeRental.UnitTest.Core
{
    public class MotorbikeServiceTests
    {
        private readonly Mock<IMotorbikeRepository> mockMotorbikeRepository;
        private readonly Mock<ICategoryRepository> mockCategoryRepository;
        private readonly Mock<IMotorbikeValidator> mockMotorbikeValidator;
        private readonly IMapper mapper;
        private readonly IMotorbikeService motorbikeService;
        public MotorbikeServiceTests()
        {
            mockMotorbikeRepository = new Mock<IMotorbikeRepository>();
            mockCategoryRepository = new Mock<ICategoryRepository>();
            mockMotorbikeValidator = new Mock<IMotorbikeValidator>();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Motorbike, MotorbikeViewModel>();
                cfg.CreateMap<MotorbikeViewModel, Motorbike>();
            });
            mapper = mapperConfiguration.CreateMapper();
            motorbikeService = new MotorbikeService(mapper, mockMotorbikeRepository.Object, mockCategoryRepository.Object, mockMotorbikeValidator.Object);
        }
        [Fact]
        public async Task CreateMotorbikeTests()
        {
            MotorbikeViewModel motorbikeViewModel = GetData();
            mockMotorbikeValidator.Setup(r => r.ValidateForCreate(motorbikeViewModel))
                .ReturnsAsync(true);
            Motorbike motorbike = mapper.Map<Motorbike>(motorbikeViewModel);
            mockMotorbikeRepository.Setup(r => r.Create(It.IsAny<Motorbike>()))
                .ReturnsAsync(motorbike);
            Category category = new Category
            {
                CategoryId = 5,
                CategoryName = "Sport"
            };
            mockCategoryRepository.Setup(r => r.GetByIdNoAsTracking(motorbike.CategoryId))
                .ReturnsAsync(category);
            MotorbikeViewModel motorbikeViewModel1 = await motorbikeService.CreateMotorbike(motorbikeViewModel);
            Assert.Equal("Sport", motorbikeViewModel1.CategoryName);
        }
        [Fact]
        public async Task DeleteMotorbikeTests()
        {
            Motorbike motorbike = mapper.Map<Motorbike>(GetData());
            mockMotorbikeRepository.Setup(r => r.GetById(1))
                .ReturnsAsync(motorbike);
            mockMotorbikeValidator.Setup(r => r.ValidateForDelete(motorbike))
                .Returns(true);
            mockMotorbikeRepository.Setup(r => r.Delete(motorbike))
                .Returns(Task.CompletedTask);
            bool result = await motorbikeService.DeleteMotorbike(1);
            Assert.Equal(true, result);
        }
        [Fact]
        public async Task GetMotorbikeByIdTest()
        {
            Motorbike motorbike = mapper.Map<Motorbike>(GetData());
            mockMotorbikeRepository.Setup(r => r.GetByIdNoAsTracking(1))
                .ReturnsAsync(motorbike);
            MotorbikeViewModel motorbikeViewModel = await motorbikeService.GetMotorbikeById(1);
            Assert.Equal(2022, motorbikeViewModel.Year);
        }
        public MotorbikeViewModel GetData()
        {
            MotorbikeViewModel motorbike = new MotorbikeViewModel
            {
                MotorbikeId = 1,
                CategoryId = 5,
                CategoryName = "Sport",
                LicensePlate = "ABC1234",
                Brand = "Honda",
                Year = 2022,
                Color = "Red",
                EngineCapacity = 150,
                ChassisNumber = "CHS1234567890",
                EngineNumber = "ENG1234567890",
                Description = "A sporty bike",
                MotorbikeConditionStatus = MotorbikeConditionStatus.New,
                ImageUrl = "https://example.com/image.jpg",
                Mileage = 1000,
                Status = MotorbikeStatus.Available
            };
            return motorbike;
        }
    }
}