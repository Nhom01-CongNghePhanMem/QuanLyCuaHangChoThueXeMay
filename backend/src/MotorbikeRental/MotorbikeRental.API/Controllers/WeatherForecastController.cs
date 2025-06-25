using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Enums.UserEnum;

namespace MotorbikeRental.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IUserCredentialsService userCredentialsService;
        private readonly RoleManager<Roles> roleManager;
        public WeatherForecastController(IUserCredentialsService userCredentialsService, RoleManager<Roles> roleManager)
        {
            this.userCredentialsService = userCredentialsService;
            this.roleManager = roleManager;
        }
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var employeeCreateDto = new EmployeeCreateDto
            {
                FullName = "Nguyễn Văn A",
                UserName = "nguyenvana",
                Password = "SecurePass123!",
                DateOfBirth = new DateTime(1999, 5, 20),
                PhoneNumber = "0912345678",
                Email = "nguyenvana@example.com",
                Address = "123 Đường ABC, Quận 1, TP.HCM",
                StartDate = DateTime.Today,
                Salary = 15000000,
                RoleId = 2, // giả sử role id = 2 là nhân viên
                Status = EmployeeStatus.Active,
                FormFile = null // nếu chưa có file upload
            };
            var result = await userCredentialsService.CreateUserCredentials(employeeCreateDto);
            return Ok(new { message = "Employee created successfully" });
        }
    }
}
