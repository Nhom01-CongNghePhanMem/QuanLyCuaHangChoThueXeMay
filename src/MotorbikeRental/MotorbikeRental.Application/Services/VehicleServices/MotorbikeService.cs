using AutoMapper;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.DTOs.Pagination;
using MotorbikeRental.Domain.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Domain.Interfaces.IServices.IVehicleServices;

namespace MotorbikeRental.Application.Services.VehicleServices
{
    public class MotorbikeService : IMotorbikeService
    {
        private readonly IMapper mapper;
        private readonly IMotorbikeRepository motorbikeRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMotorbikeValidator motorbikeValidator;
        public MotorbikeService(IMapper mapper, IMotorbikeRepository motorbikeRepository, IMotorbikeValidator motorbikeValidator, ICategoryRepository categoryRepository)
        {
            this.mapper = mapper;
            this.motorbikeRepository = motorbikeRepository;
            this.motorbikeValidator = motorbikeValidator;
            this.categoryRepository = categoryRepository;
        }

        public async Task<MotorbikeDto> CreateMotorbike(MotorbikeDto motorbikeDto)
        {
            await motorbikeValidator.ValidateForCreate(motorbikeDto);
            Motorbike motorbike = mapper.Map<Motorbike>(motorbikeDto);
            motorbike.PriceList = new PriceList { HourlyRate = motorbikeDto.HourlyRate, DailyRate = motorbikeDto.DailyRate };
            return mapper.Map<MotorbikeDto>(await motorbikeRepository.Create(motorbike));
        }

        public async Task<bool> DeleteMotorbike(int motorbikeId)
        {
            Motorbike? motorbike = await motorbikeRepository.GetById(motorbikeId);
            if (motorbike == null)
                throw new Exception("MotorBike not found");
            motorbikeValidator.ValidateForDelete(motorbike);
            await motorbikeRepository.Delete(motorbike);
            return true;
        }

        public async Task<MotorbikeIndexDto> GetMotorbikesByFilter(MotorbikeFilterDto motorbikeFilterDto)
        {
            var (data, total) = await motorbikeRepository.GetFilterData(motorbikeFilterDto);
            List<Motorbike> motorbikes = data.ToList();
            List<MotorbikeListDto> motorbikeListViewModels = new List<MotorbikeListDto>();
            for (int i = 0; i < motorbikes.Count; i++)
            {
                MotorbikeListDto motorbikeListDto = mapper.Map<MotorbikeListDto>(motorbikes[i]);
                motorbikeListDto.CategoryName = motorbikes[i].Category.CategoryName;
                motorbikeListDto.HourlyRate = motorbikes[i].PriceList.HourlyRate;
                motorbikeListDto.DailyRate = motorbikes[i].PriceList.DailyRate;
                motorbikeListViewModels.Add(motorbikeListDto);
            }
            return new MotorbikeIndexDto()
            {
                Brands = await motorbikeRepository.GetDistinctBrands(),
                CategoryViewModels = mapper.Map<IEnumerable<CategoryDto>>(await categoryRepository.GetCategoriesNoTracking()),
                PaginatedDataViewModel = new PaginatedDataDto<MotorbikeListDto>(motorbikeListViewModels, total)
            };
        }

        public async Task<MotorbikeDto> GetMotorbikeById(int id)
        {
            Motorbike motorbike = await motorbikeRepository.GetByIdWithIncludes(id);
            MotorbikeDto motorbikeDto = mapper.Map<MotorbikeDto>(motorbike);
            motorbikeDto.HourlyRate = motorbike.PriceList.HourlyRate;
            motorbikeDto.DailyRate = motorbike.PriceList.DailyRate;
            motorbikeDto.CategoryName = motorbike.Category.CategoryName;
            return motorbikeDto;
        }

        public async Task<MotorbikeDto> UpdateMotorbike(MotorbikeDto motorbikeViewModel)
        {
            await motorbikeValidator.ValidateForUpdate(motorbikeViewModel);
            Motorbike motorbike = await motorbikeRepository.GetById(motorbikeViewModel.MotorbikeId);
            if (motorbike == null)
                throw new Exception("Motorbike not found");
            motorbike.PriceList.DailyRate = motorbikeViewModel.DailyRate;
            motorbike.PriceList.HourlyRate = motorbikeViewModel.HourlyRate;
            await motorbikeRepository.Update(motorbike);
            Motorbike motorbike1 = await motorbikeRepository.GetByIdWithIncludes(motorbikeViewModel.MotorbikeId);
            MotorbikeDto motorbikeViewModel1 = mapper.Map<MotorbikeDto>(motorbike1);
            motorbikeViewModel1.CategoryName = motorbike1.Category.CategoryName;
            motorbikeViewModel1.HourlyRate = motorbike1.PriceList.HourlyRate;
            motorbikeViewModel1.DailyRate = motorbike1.PriceList.DailyRate;
            return motorbikeViewModel1;
        }
    }
}