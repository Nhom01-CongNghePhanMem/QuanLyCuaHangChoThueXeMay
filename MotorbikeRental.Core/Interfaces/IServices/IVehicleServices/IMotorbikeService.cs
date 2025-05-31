using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.Business.Pagination;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;

namespace MotorbikeRental.Core.Interfaces.IServices.IVehicleServices
{
    public interface IMotorbikeService
    {
        Task<MotorbikeViewModel> CreateMotorbike(MotorbikeViewModel motorbikeViewModel);
        Task<bool> DeleteMotorbike(int categoryId);
        Task<MotorbikeViewModel> UpdateMotorbike(MotorbikeViewModel motorbikeViewModel);
        Task<MotorbikeViewModel> GetMotorbikeById(int id);
        Task<PaginatedDataViewModel<MotorbikeListViewModel>> GetAllMotorbikes(int pageNumber, int pageSize);
        Task<PaginatedDataViewModel<MotorbikeListViewModel>> GetMotorbikesByCategoryId(int pageNumber, int pageSize, int categoryId);
    }
}