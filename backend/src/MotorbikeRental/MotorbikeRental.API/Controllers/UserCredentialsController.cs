using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorbikeRental.Application.DTOs.AuthenticDto;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
    
namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserCredentialsController : ControllerBase
    {
        private readonly IUserCredentialsService userCredentialsService;
        public UserCredentialsController(IUserCredentialsService userCredentialsService)
        {
            this.userCredentialsService = userCredentialsService;
        }
        [HttpPost]
        public async Task<IActionResult> UserCredentials([FromBody] UserCredentialsCreateDto userCredentialsCreateDtoq, CancellationToken cancellationToken = default)
        {
            var result = await userCredentialsService.CreateUserCredentials(userCredentialsCreateDtoq, cancellationToken);
            var response = new ResponseDto
            {
                Success = true,
                Message = "User credentials created successfully",
            };
            return Ok(response);
        }
        [HttpPost("{id}/reset-email")]
        public async Task<IActionResult> ResetEmail(int id ,[FromBody] ResetEmailDto resetEmailDto, CancellationToken cancellationToken = default)
        {
            if(resetEmailDto.EmployeeId != id)
            {
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Employee ID in the request body does not match the ID in the URL."
                });
            }
            var result = await userCredentialsService.ResetEmail(resetEmailDto, cancellationToken);
            var response = new ResponseDto
            {
                Success = true,
                Message = "Email reset successfully",
            };
            return Ok(response);
        }
        [HttpPost("{id}/reset-passwordByAdmin")]
        public async Task<IActionResult> ResetPasswordByAdmin(int id, [FromBody] ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken = default)
        {
            if (resetPasswordDto.EmployeeId != id)
            {
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Employee ID in the request body does not match the ID in the URL."
                });
            }
            var result = await userCredentialsService.ResetPasswordByAdmin(resetPasswordDto, cancellationToken);
            var response = new ResponseDto
            {
                Success = true,
                Message = "Password reset successfully",
            };
            return Ok(response);
        }
        [HttpPost("{id}/reset-phoneNumber")]
        public async Task<IActionResult> ResetPhoneNumber(int id, [FromBody] ResetPhoneNumberDto resetPhoneNumberDto, CancellationToken cancellationToken = default)
        {
            if (resetPhoneNumberDto.EmployeeId != id)
            {
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Employee ID in the request body does not match the ID in the URL."
                });
            }
            var result = await userCredentialsService.ResetPhoneNumber(resetPhoneNumberDto, cancellationToken);
            var response = new ResponseDto
            {
                Success = true,
                Message = "Phone number reset successfully",
            };
            return Ok(response);
        }
        [HttpPost("{id}/reset-userName")]
        public async Task<IActionResult> ResetUserName(int id, [FromBody] ResetUserNameDto resetUserNameDto, CancellationToken cancellationToken = default)
        {
            if (resetUserNameDto.EmployeeId != id)
            {
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Employee ID in the request body does not match the ID in the URL."
                });
            }
            var result = await userCredentialsService.ResetUserName(resetUserNameDto, cancellationToken);
            var response = new ResponseDto
            {
                Success = true,
                Message = "Username reset successfully",
            };
            return Ok(response);
        }
    }
}   
