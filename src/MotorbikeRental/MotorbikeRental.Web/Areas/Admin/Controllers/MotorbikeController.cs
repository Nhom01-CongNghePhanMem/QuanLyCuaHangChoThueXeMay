using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.Extensions.Caching.Memory;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Infrastructure.Constants;
using MotorbikeRental.Web.Extensions;

namespace MotorbikeRental.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MotorbikeController : Controller
    {
        private readonly ILogger<MotorbikeController> logger;
        private readonly IMotorbikeService motorbikeService;
        private readonly ICategoryService categoryService;
        private readonly IMemoryCache memoryCache;

        public MotorbikeController(ILogger<MotorbikeController> logger, IMotorbikeService motorbikeService, ICategoryService categoryService, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.motorbikeService = motorbikeService;
            this.categoryService = categoryService;
            this.memoryCache = memoryCache;
        }
        [HttpGet]
        public async Task<IActionResult> Index(MotorbikeFilterDto? filterDto)
        {
            ViewData["Title"] = "Motorbike Management";
            filterDto = filterDto.Normalize();
            try
            {
                var result = await motorbikeService.GetMotorbikesByFilter(filterDto);
                return View(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{ErrorMessages.SystemError}");
                ViewBag.Error = ErrorMessages.SystemError;
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await categoryService.GetAllCategories();
            ViewBag.MotorbikeStatuses = Enum.GetValues<MotorbikeStatus>();
            ViewBag.ConditionStatuses = Enum.GetValues<MotorbikeConditionStatus>();

            return View(new MotorbikeDto());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MotorbikeDto motorbikeDto, IFormFile formFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await categoryService.GetAllCategories();
                ViewBag.MotorbikeStatuses = Enum.GetValues<MotorbikeStatus>();
                ViewBag.ConditionStatuses = Enum.GetValues<MotorbikeConditionStatus>();
                return View(motorbikeDto);
            }
            try
            {
                var result = await motorbikeService.CreateMotorbike(motorbikeDto, formFile);
                TempData["SuccessMessage"] = "Motorbike created successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessages.SystemError);
                ViewBag.Error = ErrorMessages.SystemError;
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                MotorbikeDto motorbikeDto = new MotorbikeDto();
                if (memoryCache.TryGetValue($"Motorbike_{id}", out MotorbikeDto cachedMotorbike))
                {
                    motorbikeDto = cachedMotorbike;
                }
                else
                {
                    motorbikeDto = await motorbikeService.GetMotorbikeById(id);
                    if (motorbikeDto != null)
                        memoryCache.Set($"Motorbike_{id}", motorbikeDto, TimeSpan.FromMinutes(10));
                }
                return View(motorbikeDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessages.SystemError);
                ViewBag.Error = ErrorMessages.SystemError;
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                MotorbikeDto motorbikeDto = await motorbikeService.GetMotorbikeById(id);
                ViewBag.Categories = await categoryService.GetAllCategories();
                ViewBag.MotorbikeStatuses = Enum.GetValues<MotorbikeStatus>();
                ViewBag.ConditionStatuses = Enum.GetValues<MotorbikeConditionStatus>();
                return View(motorbikeDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessages.SystemError);
                ViewBag.Error = ErrorMessages.SystemError;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MotorbikeDto motorbikeDto, IFormFile? formFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await categoryService.GetAllCategories();
                ViewBag.MotorbikeStatuses = Enum.GetValues<MotorbikeStatus>();
                ViewBag.ConditionStatuses = Enum.GetValues<MotorbikeConditionStatus>();
                return View(motorbikeDto);
            }
            try
            {
                await motorbikeService.UpdateMotorbike(motorbikeDto, formFile);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessages.SystemError);
                ViewBag.Error = ErrorMessages.SystemError;
                return View(motorbikeDto);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await motorbikeService.DeleteMotorbike(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessages.SystemError);
                ViewBag.Error = ErrorMessages.SystemError;
                return View("Error");
            }
        }
    }
}