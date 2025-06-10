using Microsoft.AspNetCore.Http;
using MotorbikeRental.Application.DTOs.Vehicles;

namespace MotorbikeRental.Application.Interface.IServices.IVehicleServices
{
    public interface IMotorbikeService
    {
        Task<MotorbikeDto> CreateMotorbike(MotorbikeDto motorbikeDto, IFormFile formFile);
        Task<bool> DeleteMotorbike(int categoryId);
        Task<MotorbikeDto> UpdateMotorbike(MotorbikeDto motorbikeDto, IFormFile? formFile);
        Task<MotorbikeDto> GetMotorbikeById(int id);
        Task<MotorbikeIndexDto> GetMotorbikesByFilter(MotorbikeFilterDto motorbikeFilterDto);
    }
}