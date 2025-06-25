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
        public async Task<bool> ValidateForCreate(MotorbikeCreateDto motorbikeCreateDto, CancellationToken cancellationToken = default)
        {
            if (!await categoryRepository.IsExists(nameof(Category.CategoryId), motorbikeCreateDto.CategoryId, cancellationToken))
                throw new ValidatorException("Category not found");
            if (await motorbikeRepository.IsExists(nameof(Motorbike.LicensePlate), motorbikeCreateDto.LicensePlate, cancellationToken))
                throw new ValidatorException("License plate number already exists.");
            if (await motorbikeRepository.IsExists(nameof(Motorbike.ChassisNumber), motorbikeCreateDto.ChassisNumber, cancellationToken))
                throw new ValidatorException("Chassis number already exists.");
            if (await motorbikeRepository.IsExists(nameof(Motorbike.EngineNumber), motorbikeCreateDto.EngineNumber, cancellationToken))
                throw new ValidatorException("Engine number already exists");
            return true;
        }

        public bool ValidateForDelete(Motorbike motorbike)
        {
            if (motorbike.Status == MotorbikeStatus.Rented)
                throw new ValidatorException("This motorbike is currently rented and cannot be deleted.");
            return true;
        }

        public async Task<bool> ValidateForUpdate(MotorbikeUpdateDto motorbikeUpdateDto, CancellationToken cancellationToken = default)
        {
            if (!await motorbikeRepository.IsExists(nameof(Motorbike.MotorbikeId), motorbikeUpdateDto.MotorbikeId, cancellationToken))
                throw new ValidatorException("MotorBike not found");
            if (motorbikeUpdateDto.Status == MotorbikeStatus.Rented)
                throw new ValidatorException("This motorbike is currently rented and cannot be edited.");
            if (!await categoryRepository.IsExists(nameof(Category.CategoryId), motorbikeUpdateDto.CategoryId, cancellationToken))
                throw new ValidatorException("Category not found");
            if (await motorbikeRepository.IsExistsForUpdate(motorbikeUpdateDto.MotorbikeId, nameof(Motorbike.LicensePlate), motorbikeUpdateDto.LicensePlate, nameof(Motorbike.MotorbikeId), cancellationToken))
                throw new ValidatorException("License plate number already exists.");
            if (await motorbikeRepository.IsExistsForUpdate(motorbikeUpdateDto.MotorbikeId, nameof(Motorbike.ChassisNumber), motorbikeUpdateDto.ChassisNumber, nameof(Motorbike.MotorbikeId), cancellationToken))
                throw new ValidatorException("Chassis number already exists.");
            if (await motorbikeRepository.IsExistsForUpdate(motorbikeUpdateDto.MotorbikeId, nameof(Motorbike.EngineNumber), motorbikeUpdateDto.EngineNumber, nameof(Motorbike.MotorbikeId), cancellationToken))
                throw new ValidatorException("Engine number already exists");
            return true;
        }
    }
}