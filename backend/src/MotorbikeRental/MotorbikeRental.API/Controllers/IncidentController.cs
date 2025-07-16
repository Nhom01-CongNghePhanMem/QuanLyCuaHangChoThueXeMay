using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Mvc;
using MotorbikeRental.Application.DTOs.Incident;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.Interface.IServices.IIncidentServices;

namespace MotorbikeRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentService incidentService;
        public IncidentController(IIncidentService incidentService)
        {
            this.incidentService = incidentService;
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
            var incident = await incidentService.GetincidentById(id, cancellationToken);
            var response = new ResponseDto<IncidentDto>
            {
                Success = true,
                Message = "Incident retrieved successfully",
                Data = incident
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
    }
}