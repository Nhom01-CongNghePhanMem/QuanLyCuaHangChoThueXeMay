using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;

namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly ICategoryService categoryService;
        public CategoryController(IMemoryCache memoryCache, ICategoryService categoryService)
        {
            this.memoryCache = memoryCache;
            this.categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            var result = await categoryService.GetAllCategories(cancellationToken);
            var responseDto = new ResponseDto<IEnumerable<CategoryDto>>
            {
                Success = true,
                Message = "Categories retrieved successfully",
                Data = result
            };
            return Ok(responseDto);
        }
    }
}
