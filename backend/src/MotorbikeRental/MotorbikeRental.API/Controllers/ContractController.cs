using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorbikeRental.Application.DTOs.ContractDto;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.Interface.IServices.IContractServices;
using System.Threading.Tasks;

namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpPost("create-contract")]
        public async Task<IActionResult> CreateContract([FromBody] ContractCreateDto contractCreate, CancellationToken cancellation = default)
        {
            var result = await contractService.CreateContract(contractCreate, cancellation);
            var response = new ResponseDto<ContractDto>
            {
                Success = true,
                Message = "Contract created successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpPost("UpdateContractStatusActive")]
        public async Task<IActionResult> UpdateContractStatusActive([FromBody] int contractId, CancellationToken cancellation = default)
        {
            await contractService.UpdateContractStatusActive(contractId, cancellation);
            var response = new ResponseDto
            {
                Success = true,
                Message = "Contract status updated to active successfully"
            };
            return Ok(response);
        }
        [HttpPost("CompleteContractAfterReturn")]
        public async Task<IActionResult> CompleteContractAfterReturn([FromBody] int contractId, CancellationToken cancellation = default)
        {
            await contractService.CancelContractByCustomer(contractId, cancellation);
            var response = new ResponseDto
            {
                Success = true,
                Message = "Contract completed successfully after return"
            };
            return Ok(response);
        }
        [HttpPut("update-before-activation")]
        public async Task<IActionResult> UpdateContractBeforeActivation([FromBody] ContractUpdateBeforeActivationDto contractUpdate, CancellationToken cancellation = default)
        {
            var result = await contractService.UpdateContractBeforeActivation(contractUpdate, cancellation);
            var response = new ResponseDto<ContractDto>
            {
                Success = true,
                Message = "Contract updated before activation successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpGet("{id}/GetContractById")]
        public async Task<IActionResult> GetContract(int contractId, CancellationToken cancellation = default)
        {
            var result = await contractService.GetContractById(contractId, cancellation);
            var response = new ResponseDto<ContractDto>
            {
                Success = true,
                Message = "Contract retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpDelete("{id}/DeleteContract")]
        public async Task<IActionResult> DeleteContract(int contractId, CancellationToken cancellation = default)
        {
            await contractService.DeleteContract(contractId, cancellation);
            var response = new ResponseDto
            {
                Success = true,
                Message = "Contract deleted successfully"
            };
            return Ok(response);
        }
        [HttpPost("ContractSettlement")]
        public async Task<IActionResult> ContractSettlement([FromBody] ContractSettlementDto contractSettlementDto, CancellationToken cancellation = default)
        {
            var result = await contractService.ContractSettlement(contractSettlementDto, cancellation);
            var response = new ResponseDto<ContractDto>
            {
                Success = true,
                Message = "Contract settlement processed successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpGet("GetContractsByFilter")]
        public async Task<IActionResult> GetContractsByFilter([FromQuery] ContractFilterDto contractFilterDto, CancellationToken cancellation = default)
        {
            var result = await contractService.GetContractFilter(contractFilterDto, cancellation);
            var response = new ResponseDto<PaginatedDataDto<ContractListDto>>
            {
                Success = true,
                Message = "Contracts retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
    }
}
