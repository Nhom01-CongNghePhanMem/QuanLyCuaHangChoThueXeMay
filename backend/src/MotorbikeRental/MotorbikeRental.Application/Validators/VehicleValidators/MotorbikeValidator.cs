using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Application.Exceptions;
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
        public async Task<bool> ValidateForCreate(MotorbikeDto motorbikeDto, CancellationToken cancellationToken = default)
        {
            if (!await categoryRepository.IsExists(nameof(Category.CategoryId), motorbikeDto.CategoryId, cancellationToken))
                throw new ValidatorException("Category not found");
            if (await motorbikeRepository.IsExists(nameof(Motorbike.LicensePlate), motorbikeDto.LicensePlate, cancellationToken))
                throw new ValidatorException("License plate number already exists.");
            if (await motorbikeRepository.IsExists(nameof(Motorbike.ChassisNumber), motorbikeDto.ChassisNumber, cancellationToken))
                throw new ValidatorException("Chassis number already exists.");
            if (await motorbikeRepository.IsExists(nameof(Motorbike.EngineNumber), motorbikeDto.EngineNumber, cancellationToken))
                throw new ValidatorException("Engine number already exists");
            return true;
        }

        public bool ValidateForDelete(Motorbike motorbike)
        {
            if (motorbike.Status == MotorbikeStatus.Rented)
                throw new ValidatorException("This motorbike is currently rented and cannot be deleted.");
            return true;
        }

        public async Task<bool> ValidateForUpdate(MotorbikeDto motorbikeDto, CancellationToken cancellationToken = default)
        {
            if (!await motorbikeRepository.IsExists(nameof(Motorbike.MotorbikeId), motorbikeDto.MotorbikeId, cancellationToken))
                throw new ValidatorException("MotorBike not found");
            if (motorbikeDto.Status == MotorbikeStatus.Rented)
                throw new ValidatorException("This motorbike is currently rented and cannot be edited.");
            if (!await categoryRepository.IsExists(nameof(Category.CategoryId), motorbikeDto.CategoryId, cancellationToken))
                throw new ValidatorException("Category not found");
            if (await motorbikeRepository.IsExistsForUpdate(motorbikeDto.MotorbikeId, nameof(Motorbike.LicensePlate), motorbikeDto.LicensePlate, nameof(Motorbike.MotorbikeId), cancellationToken))
                throw new ValidatorException("License plate number already exists.");
            if (await motorbikeRepository.IsExistsForUpdate(motorbikeDto.MotorbikeId, nameof(Motorbike.ChassisNumber), motorbikeDto.ChassisNumber, nameof(Motorbike.MotorbikeId), cancellationToken))
                throw new ValidatorException("Chassis number already exists.");
            if (await motorbikeRepository.IsExistsForUpdate(motorbikeDto.MotorbikeId, nameof(Motorbike.EngineNumber), motorbikeDto.EngineNumber, nameof(Motorbike.MotorbikeId), cancellationToken))
                throw new ValidatorException("Engine number already exists");
            return true;
        }
    }
}