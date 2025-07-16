using MotorbikeRental.Application.DTOs.Incident;

namespace MotorbikeRental.Application.Interface.IServices.IIncidentServices
{
    public interface IIncidentService
    {
        Task<IncidentDto> CreateIncident(IncidentCreateDto incidentCreateDto, CancellationToken cancellationToken = default);
        Task<IncidentDto> UpdateBeforeComplete(IncidentUpdateBeforeCompleteDto incidentUpdateBeforeCompleteDto, CancellationToken cancellationToken = default);
        Task<IncidentDto> GetincidentById(int id, CancellationToken cancellation = default);
        Task<bool> DeleteIncident(int incidentId, CancellationToken cancellationToken = default);
    }
}
