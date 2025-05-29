using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using MotorbikeRental.Core.Entities.Business.Pagination;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Core.Interfaces.IServices.IVehicleServices;

namespace MotorbikeRental.Core.Services.VehicleServices
{
    public class MotorbikeService : IMotorbikeService
    {
        private readonly IMapper mapper;
        private readonly IMotorbikeRepository motorbikeRepository;
        private readonly ICategoryRepository categoryRepository;
        public MotorbikeService(IMapper mapper, IMotorbikeRepository motorbikeRepository, ICategoryRepository categoryRepository)
        {
            this.mapper = mapper;
            this.motorbikeRepository = motorbikeRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<MotorbikeViewModel> CreateMotorbike(MotorbikeViewModel motorbikeViewModel)
        {
            if (!await categoryRepository.CategoryIdExists(motorbikeViewModel.CategoryId))
                throw new Exception("Category not found");
            if (await motorbikeRepository.LicensePlateExists(motorbikeViewModel.LicensePlate))
                throw new Exception("License plate number already exists.");
            if (await motorbikeRepository.ChassisNumberExists(motorbikeViewModel.ChassisNumber))
                throw new Exception("Chassis number already exists.");
            if (await motorbikeRepository.EngineNumberExists(motorbikeViewModel.EngineNumber))
                throw new Exception("Engine number already exists");
            Motorbike motorbike = await motorbikeRepository.Create(mapper.Map<Motorbike>(motorbikeViewModel));
            return mapper.Map<MotorbikeViewModel>(motorbike);
        }
        public async Task<bool> DeleteMotorbike(int motorbikeId)
        {
            if (!await motorbikeRepository.IsExists(motorbikeId))
            {
                throw new Exception("MotorBike not found");
            }
            return true;
        }

        public Task<PaginatedDataViewModel<CategoryViewModel>> GetAllMotorbikes()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryViewModel> GetMotorbikeById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryViewModel> UpdateMotorbike(MotorbikeViewModel motorbikeViewModel)
        {
            throw new NotImplementedException();
        }
    }
}