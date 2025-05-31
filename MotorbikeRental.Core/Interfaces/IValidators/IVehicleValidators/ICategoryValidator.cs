using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.Business.Vehicles;

namespace MotorbikeRental.Core.Interfaces.IValidators.IVehicleValidators
{
    public interface ICategoryValidator
    {
        Task<bool> ValidateForCreate(CategoryViewModel categoryViewModel);
        Task<bool> ValidateForDelete(int id);
        Task<bool> ValidateForUpdate(CategoryViewModel categoryViewModel);
        Task<bool> ValidateForGet(int id);
    }
}