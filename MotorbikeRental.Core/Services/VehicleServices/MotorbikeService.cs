using AutoMapper;
using MotorbikeRental.Core.Entities.Business.Pagination;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Entities.General.Pricing;
using MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Core.Interfaces.IServices.IVehicleServices;
using MotorbikeRental.Core.Interfaces.IValidators.IVehicleValidators;

namespace MotorbikeRental.Core.Services.VehicleServices
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

        public async Task<MotorbikeViewModel> CreateMotorbike(MotorbikeViewModel motorbikeViewModel)
        {
            await motorbikeValidator.ValidateForCreate(motorbikeViewModel);
            Motorbike motorbike = mapper.Map<Motorbike>(motorbikeViewModel);
            motorbike.PriceList = new PriceList { HourlyRate = motorbikeViewModel.HourlyRate, DailyRate = motorbikeViewModel.DailyRate };
            return mapper.Map<MotorbikeViewModel>(await motorbikeRepository.Create(motorbike));
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

        public async Task<MotorbikeIndexViewModel> GetMotorbikesByFilter(MotorbikeFilterViewModel motorbikeFilterViewModel)
        {
            var (data, total) = await motorbikeRepository.GetFilterData(motorbikeFilterViewModel);
            List<Motorbike> motorbikes = data.ToList();
            List<MotorbikeListViewModel> motorbikeListViewModels = new List<MotorbikeListViewModel>();
            for (int i = 0; i < motorbikes.Count; i++)
            {
                MotorbikeListViewModel motorbikeListViewModel = mapper.Map<MotorbikeListViewModel>(motorbikes[i]);
                motorbikeListViewModel.CategoryName = motorbikes[i].Category.CategoryName;
                motorbikeListViewModel.HourlyRate = motorbikes[i].PriceList.HourlyRate;
                motorbikeListViewModel.DailyRate = motorbikes[i].PriceList.DailyRate;
                motorbikeListViewModels.Add(motorbikeListViewModel);
            }
            return new MotorbikeIndexViewModel
            {
                Brands = await motorbikeRepository.GetDistinctBrands(),
                CategoryViewModels = mapper.Map<IEnumerable<CategoryViewModel>>(await categoryRepository.GetCategoriesNoTracking()),
                PaginatedDataViewModel = new PaginatedDataViewModel<MotorbikeListViewModel>(motorbikeListViewModels, total)
            };
        }

        public async Task<MotorbikeViewModel> GetMotorbikeById(int id)
        {
            Motorbike motorbike = await motorbikeRepository.GetByIdWithIncludes(id);
            MotorbikeViewModel motorbikeViewModel = mapper.Map<MotorbikeViewModel>(motorbike);
            motorbikeViewModel.HourlyRate = motorbike.PriceList.HourlyRate;
            motorbikeViewModel.DailyRate = motorbike.PriceList.DailyRate;
            motorbikeViewModel.CategoryName = motorbike.Category.CategoryName;
            return motorbikeViewModel;
        }

        public async Task<MotorbikeViewModel> UpdateMotorbike(MotorbikeViewModel motorbikeViewModel)
        {
            await motorbikeValidator.ValidateForUpdate(motorbikeViewModel);
            Motorbike motorbike = await motorbikeRepository.GetById(motorbikeViewModel.MotorbikeId);
            if (motorbike == null)
                throw new Exception("Motorbike not found");
            motorbike.PriceList.DailyRate = motorbikeViewModel.DailyRate;
            motorbike.PriceList.HourlyRate = motorbikeViewModel.HourlyRate;
            await motorbikeRepository.Update(motorbike);
            Motorbike motorbike1 = await motorbikeRepository.GetByIdWithIncludes(motorbikeViewModel.MotorbikeId);
            MotorbikeViewModel motorbikeViewModel1 = mapper.Map<MotorbikeViewModel>(motorbike1);
            motorbikeViewModel1.CategoryName = motorbike1.Category.CategoryName;
            motorbikeViewModel1.HourlyRate = motorbike1.PriceList.HourlyRate;
            motorbikeViewModel1.DailyRate = motorbike1.PriceList.DailyRate;
            return motorbikeViewModel1;
        }
    }
}