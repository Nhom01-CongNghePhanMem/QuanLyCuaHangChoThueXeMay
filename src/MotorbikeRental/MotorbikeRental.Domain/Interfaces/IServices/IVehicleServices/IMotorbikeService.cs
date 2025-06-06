using MotorbikeRental.Domain.DTOs.Vehicles;

namespace MotorbikeRental.Domain.Interfaces.IServices.IVehicleServices
{
    public interface IMotorbikeService
    {
        Task<MotorbikeDto> CreateMotorbike(MotorbikeDto motorbikeDto);
        Task<bool> DeleteMotorbike(int categoryId);
        Task<MotorbikeDto> UpdateMotorbike(MotorbikeDto motorbikeDto);
        Task<MotorbikeDto> GetMotorbikeById(int id);
        Task<MotorbikeIndexDto> GetMotorbikesByFilter(MotorbikeFilterDto motorbikeFilterDto);
    }
}