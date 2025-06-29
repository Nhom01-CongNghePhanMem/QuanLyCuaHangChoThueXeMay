using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotorbikeRental.Application.DTOs.AuthenticDto;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.Interface.IServices.IAuthServices;
using MotorbikeRental.Domain.Entities.User;
using System.Threading.Tasks;

namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IJwtTokenService jwtTokenService;
        private readonly RoleManager<Roles> roleManager;
        public AuthController(IAuthService authService, IJwtTokenService jwtTokenService, RoleManager<Roles> roleManager)
        {
            this.authService = authService;
            this.jwtTokenService = jwtTokenService;
            this.roleManager = roleManager;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            var result = await authService.Login(loginDto, cancellationToken);
            if (result == null)
            {
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                });
            }
            string token = jwtTokenService.GenerateJwtToken(result);
            var response = new ResponseDto<string>
            {
                Success = true,
                Message = "Login successful",
                Data = token
            };
            return Ok(response);
        }
    }
}
