using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorbikeRental.Application.DTOs.ContractDto;
using MotorbikeRental.Application.Interface.IServices.IContractServices;

namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractController : ControllerBase
    {
        private readonly IContractService contractService;
        public ContractController(IContractService contractService)
        {
            this.contractService = contractService;
        }
        [HttpPost("calculate-price")]
        public async Task<IActionResult> CalculateRentalPrice([FromBody] RentalPriceRequestDto priceRequestDto, CancellationToken cancellationToken = default)
        {
            var result = await contractService.CalculateRentalPrice(priceRequestDto, cancellationToken);
            return Ok(result);
        }
    }
}
