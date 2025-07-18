using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MotorbikeRental.Application.DTOs.Incident;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.Interface.IServices.IIncidentServices;

namespace MotorbikeRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentService incidentService;
        private readonly IMemoryCache memoryCache;
        public IncidentController(IIncidentService incidentService, IMemoryCache memoryCache)
        {
            this.incidentService = incidentService;
            this.memoryCache = memoryCache;
        }
        [HttpPost]
        public async Task<IActionResult> CreateIncident([FromForm] IncidentCreateDto incidentCreateDto, CancellationToken cancellationToken = default)
        {
            var result = await incidentService.CreateIncident(incidentCreateDto, cancellationToken);
            var response = new ResponseDto<IncidentDto>
            {
                Success = true,
                Message = "Incident create successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpPut("before-complete")]
        public async Task<IActionResult> UpdateBeforeComplete([FromForm] IncidentUpdateBeforeCompleteDto incidentUpdateBeforeCompleteDto, CancellationToken cancellationToken = default)
        {
            var result = await incidentService.UpdateBeforeComplete(incidentUpdateBeforeCompleteDto, cancellationToken);
            memoryCache.Remove($"Incident_{result.IncidentId}");
            var response = new ResponseDto<IncidentDto>
            {
                Success = true,
                Message = "Incident updated successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpGet("{id}/GetIncidentById")]
        public async Task<IActionResult> GetIncidentById(int id, CancellationToken cancellationToken = default)
        {
            var result = new IncidentDto();
            if (!memoryCache.TryGetValue($"Incident_{id}", out IncidentDto? incidentDto))
            {
                result = incidentDto;
            }
            else
            {
                result = await incidentService.GetIncidentById(id, cancellationToken);
                if (result != null)
                    memoryCache.Set($"Incident_{id}", result, TimeSpan.FromMinutes(10));
            }
            var response = new ResponseDto<IncidentDto>
            {
                Success = true,
                Message = "Incident retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpDelete("{incidentId}/DeleteIncident")]
        public async Task<IActionResult> DeleteIncident(int incidentId, CancellationToken cancellationToken = default)
        {
            bool result = await incidentService.DeleteIncident(incidentId, cancellationToken);
            var response = new ResponseDto
            {
                Success = result,
                Message = result ? "Incident deleted successfully" : "Failed to delete incident"
            };
            return Ok(response);
        }
        [HttpGet("GetIncidentsByFilter")]
        public async Task<IActionResult> GetIncidentsByFilter([FromQuery] IncidentFilterDto incidentFilterDto, CancellationToken cancellationToken = default)
        {
            var result = await incidentService.GetIncidentsByFilter(incidentFilterDto, cancellationToken);
            var response = new ResponseDto<PaginatedDataDto<IncidentListDto>>
            {
                Success = true,
                Message = "Incidents retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpPost("CompleteIncident")]
        public async Task<IActionResult> CompleteIncident([FromForm] IncidentCompleteDto incidentCompleteDto, CancellationToken cancellationToken = default)
        {
            var result = await incidentService.CompleteIncident(incidentCompleteDto, cancellationToken);
            memoryCache.Remove($"Incident_{result.IncidentId}");
            var response = new ResponseDto<IncidentDto>
            {
                Success = true,
                Message = "Incident completed successfully",
                Data = result
            };
            return Ok(response);
        }
    }
}