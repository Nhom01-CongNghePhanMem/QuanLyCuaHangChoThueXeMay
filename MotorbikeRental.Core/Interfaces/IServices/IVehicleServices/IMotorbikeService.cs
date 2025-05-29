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
        Task<CategoryViewModel> UpdateMotorbike(MotorbikeViewModel motorbikeViewModel);
        Task<CategoryViewModel> GetMotorbikeById(int id);
        Task<PaginatedDataViewModel<CategoryViewModel>> GetAllMotorbikes();
    }
}