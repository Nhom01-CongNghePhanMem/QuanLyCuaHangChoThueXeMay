using Microsoft.AspNetCore.Mvc;
using MotorbikeRental.Application.DTOs.ContractDto;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.Interface.IServices.IContractServices;

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
        [HttpPost]
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
        [HttpPost("{id}/active")]
        public async Task<IActionResult> UpdateContractStatusActive([FromBody] int id, CancellationToken cancellation = default)
        {
            await contractService.UpdateContractStatusActive(id, cancellation);
            var response = new ResponseDto
            {
                Success = true,
                Message = "Contract status updated to active successfully"
            };
            return Ok(response);
        }
        [HttpPost("{id}/cancel-contract")]
        public async Task<IActionResult> CancelContractByCustomer([FromBody] int id, CancellationToken cancellation = default)
        {
            await contractService.CancelContractByCustomer(id, cancellation);
            var response = new ResponseDto
            {
                Success = true,
                Message = "Contract completed successfully after return"
            };
            return Ok(response);
        }
        [HttpPut("{id}/before-activation")]
        public async Task<IActionResult> UpdateContractBeforeActivation(int id, [FromBody] ContractUpdateBeforeActivationDto contractUpdate, CancellationToken cancellation = default)
        {
            if(id != contractUpdate.ContractId)
                return BadRequest("Contract ID mismatch");
            var result = await contractService.UpdateContractBeforeActivation(contractUpdate, cancellation);
            var response = new ResponseDto<ContractDto>
            {
                Success = true,
                Message = "Contract updated before activation successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContract(int id, CancellationToken cancellation = default)
        {
            var result = await contractService.GetContractById(id, cancellation);
            var response = new ResponseDto<ContractDto>
            {
                Success = true,
                Message = "Contract retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpDelete("{id}")]
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
        [HttpPost("{id}/settlement")]
        public async Task<IActionResult> ContractSettlement(int id, [FromBody] ContractSettlementDto contractSettlementDto, CancellationToken cancellation = default)
        {
            if (id != contractSettlementDto.ContractId)
                return BadRequest("Contract ID mismatch");
            var result = await contractService.ContractSettlement(contractSettlementDto, cancellation);
            var response = new ResponseDto<ContractDto>
            {
                Success = true,
                Message = "Contract settlement processed successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpGet]
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
