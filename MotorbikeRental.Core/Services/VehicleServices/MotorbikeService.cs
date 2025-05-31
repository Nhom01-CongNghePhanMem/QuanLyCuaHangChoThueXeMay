using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using MotorbikeRental.Core.Entities.Business.Pagination;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Enums;
using MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Core.Interfaces.IServices.IVehicleServices;
using MotorbikeRental.Core.Interfaces.IValidators.IVehicleValidators;

namespace MotorbikeRental.Core.Services.VehicleServices
{
    public class MotorbikeService : IMotorbikeService
    {
        private readonly IMapper mapper;
        private readonly IMotorbikeRepository motorbikeRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMotorbikeValidator motorbikeValidator;
        public MotorbikeService(IMapper mapper, IMotorbikeRepository motorbikeRepository, ICategoryRepository categoryRepository, IMotorbikeValidator motorbikeValidator)
        {
            this.mapper = mapper;
            this.motorbikeRepository = motorbikeRepository;
            this.categoryRepository = categoryRepository;
            this.motorbikeValidator = motorbikeValidator;
        }

        public async Task<MotorbikeViewModel> CreateMotorbike(MotorbikeViewModel motorbikeViewModel)
        {
            await motorbikeValidator.ValidateForCreate(motorbikeViewModel);
            Motorbike motorbike = await motorbikeRepository.Create(mapper.Map<Motorbike>(motorbikeViewModel));
            Category category = await categoryRepository.GetByIdNoAsTracking(motorbike.CategoryId);
            MotorbikeViewModel motorbikeViewModel1 = mapper.Map<MotorbikeViewModel>(motorbike);
            motorbikeViewModel1.CategoryName = category.CategoryName;
            return motorbikeViewModel1;
        }
        public async Task<bool> DeleteMotorbike(int motorbikeId)
        {
            Motorbike? motorbike = await motorbikeRepository.GetById(motorbikeId);
            if (motorbike == null)
                throw new Exception("MotorBike not found");
            motorbikeValidator.ValidateForDelete(motorbike);
            await motorbikeRepository.Delete(motorbike);
            return true;
        }

        public async Task<PaginatedDataViewModel<MotorbikeListViewModel>> GetAllMotorbikes(int pageNumber, int pageSize)
        {
            PaginatedDataViewModel<Motorbike> paginatedDataView = await motorbikeRepository.GetPaginatedData(pageNumber, pageSize);
            return new PaginatedDataViewModel<MotorbikeListViewModel>(mapper.Map<IEnumerable<MotorbikeListViewModel>>(paginatedDataView.Data), paginatedDataView.TotalCount);
        }

        public async Task<MotorbikeViewModel> GetMotorbikeById(int id)
        {
            return mapper.Map<MotorbikeViewModel>(await motorbikeRepository.GetByIdNoAsTracking(id));
        }

        public async Task<MotorbikeViewModel> UpdateMotorbike(MotorbikeViewModel motorbikeViewModel)
        {
            await motorbikeValidator.ValidateForUpdate(motorbikeViewModel);
            Motorbike motorbike = mapper.Map<Motorbike>(motorbikeViewModel);
            await motorbikeRepository.Update(motorbike);
            return mapper.Map<MotorbikeViewModel>(motorbike);
        }
        public async Task<PaginatedDataViewModel<MotorbikeListViewModel>> GetMotorbikesByCategoryId(int pageNumber, int pageSize, int categoryId)
        {
            if (!await categoryRepository.CategoryIdExists(categoryId))
                throw new Exception("Category not found");
            PaginatedDataViewModel<Motorbike> paginatedDataView = await motorbikeRepository.GetMotorbikesByCategory(pageNumber, pageSize, categoryId);
            return new PaginatedDataViewModel<MotorbikeListViewModel>(mapper.Map<IEnumerable<MotorbikeListViewModel>>(paginatedDataView.Data), paginatedDataView.TotalCount);
        }
        }
}