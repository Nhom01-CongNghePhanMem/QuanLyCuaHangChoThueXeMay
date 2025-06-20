using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;
using MotorbikeRental.Web.Extensions;

namespace MotorbikeRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorbikeController : ControllerBase
    {
        private readonly IMotorbikeService motorbikeService;
        private readonly IMemoryCache memoryCache;

        public MotorbikeController(IMotorbikeService motorbikeService, IMemoryCache memoryCache)
        {
            this.motorbikeService = motorbikeService;
            this.memoryCache = memoryCache;
        }
        [HttpGet]
        public async Task<IActionResult> GetMotorbikeByFilter([FromQuery] MotorbikeFilterDto? filterDto, CancellationToken cancellationToken)
        {
            filterDto = filterDto.Normalize();
            var result = await motorbikeService.GetMotorbikesByFilter(filterDto, cancellationToken);
            var responseDto = new ResponseDto<PaginatedDataDto<MotorbikeListDto>>
            {
                Success = true,
                Message = "Motorbikes retrieved successfully",
                Data = result
            };
            return Ok(responseDto);
        }
        [HttpGet("filter-options")]
        public async Task<IActionResult> GetMotorbikeFilterOptions(CancellationToken cancellationToken)
        {
            var result = await motorbikeService.GetMotorbikeFilterOptions(cancellationToken);
            var responseDto = new ResponseDto<MotorbikeIndexDto>
            {
                Success = true,
                Message = "Motorbikes options retrieved successfully",
                Data = result
            };
            return Ok(responseDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotorbikeById(int id, CancellationToken cancellationToken)
        {
            var result = new MotorbikeDto();
            if (memoryCache.TryGetValue($"Motorbike_{id}", out MotorbikeDto cacheMotorbike))
            {
                result = cacheMotorbike;
            }
            else
            {
                result = await motorbikeService.GetMotorbikeById(id, cancellationToken);
                if (result != null)
                    memoryCache.Set($"Motorbike_{id}", result, TimeSpan.FromMinutes(10));
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMotorbike([FromForm] MotorbikeDto motorbikeDto, CancellationToken cancellationToken)
        {
            var result = await motorbikeService.CreateMotorbike(motorbikeDto, cancellationToken);
            var response = new ResponseDto<MotorbikeDto>
            {
                Success = true,
                Message = "Motorbike create successfully",
                Data = result
            };
            return CreatedAtAction(nameof(GetMotorbikeById), new { id = result.MotorbikeId }, response);
        }
        [HttpPut]
        public async Task<IActionResult> EditMotorbike([FromForm] MotorbikeDto motorbikeDto, CancellationToken cancellationToken)
        {
            var result = await motorbikeService.UpdateMotorbike(motorbikeDto, cancellationToken);
            var response = new ResponseDto<MotorbikeDto>
            {
                Success = true,
                Message = "Motorbike update successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMotorbike(int id, CancellationToken cancellationToken)
        {
            await motorbikeService.DeleteMotorbike(id, cancellationToken);
            return Ok(new ResponseDto
            {
                Success = true,
                Message = "Motorbike delete successfully"
            });
        }
    }
}
