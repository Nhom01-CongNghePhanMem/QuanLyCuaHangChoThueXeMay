using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Enums;
using MotorbikeRental.Core.Enums.VehicleEnum;
using MotorbikeRental.Infrastructure.Data;
using MotorbikeRental.Infrastructure.Repositories;
using MotorbikeRental.Infrastructure.Repositories.VehicleRepositories;

namespace MotorbikeRental.UnitTest.Infrastructure
{
    public class MotorbikeRepositoryTests
    {
        Motorbike motorbike = new Motorbike()
        {
            MotorbikeId = 0,
            CategoryId = 1,
            LicensePlate = "36A3436446",
            Brand = "Honda",
            Year = 1990,
            Color = "Red",
            EngineCapacity = 110,
            ChassisNumber = "254563456",
            EngineNumber = "54563456",
            Description = "",
            MotorbikeConditionStatus = MotorbikeConditionStatus.LikeNew,
            ImageUrl = "55656jpg",
            Mileage = 43456456,
            Status = MotorbikeStatus.Available,
        };


        private MotorbikeRentalDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MotorbikeRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_" + Guid.NewGuid().ToString())
                .Options;
            return new MotorbikeRentalDbContext(options);
        }
        [Fact]
        public async Task CreateMotorbikeTest()
        {
            MotorbikeRepository motorbikeRepository = new MotorbikeRepository(GetInMemoryDbContext());
            Motorbike motorbike1 = await motorbikeRepository.Create(motorbike);
            Motorbike motorbike2 = new Motorbike();
            Assert.Equal(motorbike, motorbike1);
        }
        [Fact]
        public async Task GetByIdTests()
        {
            MotorbikeRepository motorbikeRepository = new MotorbikeRepository(GetInMemoryDbContext());
            Motorbike motorbike = await motorbikeRepository.GetById(1);
            Assert.Equal(motorbike, motorbike);
        }
        [Fact]
        public async Task CreateMotorbikeTest1()
        {
            BaseRepository<Motorbike> baseRepository = new BaseRepository<Motorbike>(GetInMemoryDbContext());
            Motorbike motorbike1 = await baseRepository.Create(motorbike);
            Motorbike motorbike2 = new Motorbike();
            Assert.Equal(motorbike, motorbike1);
        }
    }
}