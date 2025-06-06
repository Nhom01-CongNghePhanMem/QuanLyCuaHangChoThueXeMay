using MotorbikeRental.Domain.DTOs.Vehicles;

namespace MotorbikeRental.Application.Interface.IValidators.IVehicleValidators
{
    public interface ICategoryValidator
    {
        Task<bool> ValidateForCreate(CategoryDto categoryDto);
        Task<bool> ValidateForDelete(int id);
        Task<bool> ValidateForUpdate(CategoryDto categoryDto);
        Task<bool> ValidateForGet(int id);
    }
}