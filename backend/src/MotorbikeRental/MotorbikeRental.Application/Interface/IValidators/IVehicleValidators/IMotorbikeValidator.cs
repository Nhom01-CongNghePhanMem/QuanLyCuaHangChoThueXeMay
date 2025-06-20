using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Application.Interface.IValidators.IVehicleValidators
{
    public interface IMotorbikeValidator
    {
        Task<bool> ValidateForCreate(MotorbikeDto motorbikeViewModel, CancellationToken cancellationToken = default);
        bool ValidateForDelete(Motorbike motorbike);
        Task<bool> ValidateForUpdate(MotorbikeDto motorbikeDto, CancellationToken cancellationToken = default);
    }
}