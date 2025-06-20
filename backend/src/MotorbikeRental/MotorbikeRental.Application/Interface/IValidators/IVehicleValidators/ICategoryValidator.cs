using MotorbikeRental.Application.DTOs.Vehicles;

namespace MotorbikeRental.Application.Interface.IValidators.IVehicleValidators
{
    public interface ICategoryValidator
    {
        Task<bool> ValidateForCreate(CategoryDto categoryDto, CancellationToken cancellationToken = default);
        Task<bool> ValidateForDelete(int id, CancellationToken cancellationToken = default);
        Task<bool> ValidateForUpdate(CategoryDto categoryDto, CancellationToken cancellationToken = default);
        Task<bool> ValidateForGet(int id, CancellationToken cancellationToken = default);
    }
}