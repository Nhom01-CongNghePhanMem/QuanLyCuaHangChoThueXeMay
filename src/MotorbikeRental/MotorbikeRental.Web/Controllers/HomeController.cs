using Microsoft.AspNetCore.Mvc;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Web.Models;
using System.Diagnostics;

namespace MotorbikeRental.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRoleService roleService;

        public HomeController(ILogger<HomeController> logger, IRoleService roleService)
        {
            _logger = logger;
            this.roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> ListRoles()
        {
            return View(await roleService.GetAllRoles());
        }
    }
}
