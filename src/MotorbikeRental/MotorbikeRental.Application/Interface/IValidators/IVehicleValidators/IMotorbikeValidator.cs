using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Application.Interface.IValidators.IVehicleValidators
{
    public interface IMotorbikeValidator
    {
        Task<bool> ValidateForCreate(MotorbikeDto motorbikeViewModel);
        bool ValidateForDelete(Motorbike motorbike);
        Task<bool> ValidateForUpdate(MotorbikeDto motorbikeDto);
    }
}