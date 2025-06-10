using AutoMapper;
using Microsoft.AspNetCore.Http;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IExternalServices.Storage;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;

namespace MotorbikeRental.Application.Services.VehicleServices
{
    public class MotorbikeService : IMotorbikeService
    {
        private readonly IMapper mapper;
        private readonly IMotorbikeRepository motorbikeRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMotorbikeValidator motorbikeValidator;
        private readonly IFileService fileService;
        public MotorbikeService(IMapper mapper, IMotorbikeRepository motorbikeRepository, IMotorbikeValidator motorbikeValidator, ICategoryRepository categoryRepository, IFileService fileService)
        {
            this.mapper = mapper;
            this.motorbikeRepository = motorbikeRepository;
            this.motorbikeValidator = motorbikeValidator;
            this.categoryRepository = categoryRepository;
            this.fileService = fileService;
        }

        public async Task<MotorbikeDto> CreateMotorbike(MotorbikeDto motorbikeDto, IFormFile formFile)
        {
            await motorbikeValidator.ValidateForCreate(motorbikeDto);
            Motorbike motorbike = mapper.Map<Motorbike>(motorbikeDto);
            motorbike.PriceList = new PriceList { HourlyRate = motorbikeDto.HourlyRate, DailyRate = motorbikeDto.DailyRate };
            motorbike.ImageUrl = await fileService.SaveImage(formFile, "Motorbike");
            return mapper.Map<MotorbikeDto>(await motorbikeRepository.Create(motorbike));
        }

        public async Task<bool> DeleteMotorbike(int motorbikeId)
        {
            Motorbike? motorbike = await motorbikeRepository.GetById(motorbikeId);
            if (motorbike == null)
                throw new Exception("MotorBike not found");
            motorbikeValidator.ValidateForDelete(motorbike);
            await motorbikeRepository.Delete(motorbike);
            fileService.DeleteFile(motorbike.ImageUrl);
            return true;
        }

        public async Task<MotorbikeIndexDto> GetMotorbikesByFilter(MotorbikeFilterDto motorbikeFilterDto)
        {
                var (data, total) = await motorbikeRepository.GetFilterData(motorbikeFilterDto.CategoryId, motorbikeFilterDto.Brand, motorbikeFilterDto.Status, motorbikeFilterDto.PageNumber, motorbikeFilterDto.PageSize);
                List<Motorbike> motorbikes = data.ToList();
                List<MotorbikeListDto> motorbikeListDto = new List<MotorbikeListDto>();
                for (int i = 0; i < motorbikes.Count; i++)
                {
                    motorbikeListDto.Add(mapper.Map<MotorbikeListDto>(motorbikes[i]));
                }
                return new MotorbikeIndexDto()
                {
                    Brands = await motorbikeRepository.GetDistinctBrands(),
                    CategoryViewModels = mapper.Map<IEnumerable<CategoryDto>>(await categoryRepository.GetCategoriesNoTracking()),
                    PaginatedDataViewModel = new PaginatedDataDto<MotorbikeListDto>(motorbikeListDto, total)
                };
        }

        public async Task<MotorbikeDto> GetMotorbikeById(int id)
        {
            return mapper.Map<MotorbikeDto>(await motorbikeRepository.GetByIdWithIncludes(id));
        }

        public async Task<MotorbikeDto> UpdateMotorbike(MotorbikeDto motorbikeDto, IFormFile? formFile)
        {
            await motorbikeValidator.ValidateForUpdate(motorbikeDto);
            Motorbike motorbike = await motorbikeRepository.GetByIdWithIncludes(motorbikeDto.MotorbikeId);
            if (motorbike == null)
                throw new Exception("Motorbike not found");
            motorbike.PriceList.DailyRate = motorbikeDto.DailyRate;
            motorbike.PriceList.HourlyRate = motorbikeDto.HourlyRate;
            if (formFile != null)
                motorbike.ImageUrl = await fileService.SaveImage(formFile, "motorbikes");
            await motorbikeRepository.Update(motorbike);
            return mapper.Map<MotorbikeDto>(motorbike);
        }
    }
}