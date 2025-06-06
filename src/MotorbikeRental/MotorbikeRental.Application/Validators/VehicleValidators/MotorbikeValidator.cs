using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;

namespace MotorbikeRental.Application.Validators.VehicleValidators
{
    public class MotorbikeValidator : IMotorbikeValidator
    {
        private readonly IMotorbikeRepository motorbikeRepository;
        private readonly ICategoryRepository categoryRepository;
        public MotorbikeValidator(IMotorbikeRepository motorbikeRepository, ICategoryRepository categoryRepository)
        {
            this.motorbikeRepository = motorbikeRepository;
            this.categoryRepository = categoryRepository;
        }
        public async Task<bool> ValidateForCreate(MotorbikeDto motorbikeViewModel)
        {
            if (!await categoryRepository.CategoryIdExists(motorbikeViewModel.CategoryId))
                throw new Exception("Category not found");
            if (await motorbikeRepository.LicensePlateExists(motorbikeViewModel.LicensePlate))
                throw new Exception("License plate number already exists.");
            if (await motorbikeRepository.ChassisNumberExists(motorbikeViewModel.ChassisNumber))
                throw new Exception("Chassis number already exists.");
            if (await motorbikeRepository.EngineNumberExists(motorbikeViewModel.EngineNumber))
                throw new Exception("Engine number already exists");
            return true;
        }

        public bool ValidateForDelete(Motorbike motorbike)
        {
            if (motorbike.Status == MotorbikeStatus.Rented)
                throw new Exception("This motorbike is currently rented and cannot be deleted.");
            return true;
        }

        public async Task<bool> ValidateForUpdate(MotorbikeDto motorbikeViewModel)
        {
            if (!await motorbikeRepository.IsExists(motorbikeViewModel.MotorbikeId))
                throw new Exception("MotorBike not found");
            if (motorbikeViewModel.Status == MotorbikeStatus.Rented)
                throw new Exception("This motorbike is currently rented and cannot be edited.");
            if (!await categoryRepository.CategoryIdExists(motorbikeViewModel.CategoryId))
                throw new Exception("Category not found");
            if (await motorbikeRepository.DupLicensePlateExceptId(motorbikeViewModel.LicensePlate, motorbikeViewModel.MotorbikeId))
                throw new Exception("License plate number already exists.");
            if (await motorbikeRepository.DupChassisNumExceptId(motorbikeViewModel.ChassisNumber, motorbikeViewModel.MotorbikeId))
                throw new Exception("Chassis number already exists.");
            if (await motorbikeRepository.DupEngineNumExceptId(motorbikeViewModel.EngineNumber, motorbikeViewModel.MotorbikeId))
                throw new Exception("Engine number already exists");
            return true;
        }
    }
}