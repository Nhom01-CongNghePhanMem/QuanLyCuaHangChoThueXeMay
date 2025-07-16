using MotorbikeRental.Application.DTOs.Incident;
using MotorbikeRental.Domain.Entities.Contract;

namespace MotorbikeRental.Application.Interface.IValidators.IIncidentValidators
{
    public interface IIncidentValidator
    {
        Task<bool> ValidateForCreate(IncidentCreateDto incidentCreateDto, RentalContract rentalContract, CancellationToken cancellationToken = default);
        Task<bool> ValidateForUpdateBeforeActivation(IncidentUpdateBeforeCompleteDto incidentUpdateBeforeCompleteDto, RentalContract rentalContract, CancellationToken cancellationToken = default);
    }
}