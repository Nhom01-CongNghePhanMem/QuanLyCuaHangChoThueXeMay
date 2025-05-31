using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;

namespace MotorbikeRental.Core.Interfaces.IValidators.IVehicleValidators
{
    public interface IMotorbikeValidator
    {
        Task<bool> ValidateForCreate(MotorbikeViewModel motorbikeViewModel);
        bool ValidateForDelete(Motorbike motorbike);
        Task<bool> ValidateForUpdate(MotorbikeViewModel motorbikeViewModel);
    }
}