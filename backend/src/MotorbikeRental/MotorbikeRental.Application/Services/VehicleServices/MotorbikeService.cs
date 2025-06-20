using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Exceptions;
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

        public async Task<MotorbikeDto> CreateMotorbike(MotorbikeDto motorbikeDto, CancellationToken cancellationToken = default)
        {
            await motorbikeValidator.ValidateForCreate(motorbikeDto, cancellationToken);
            Motorbike motorbike = mapper.Map<Motorbike>(motorbikeDto);
            motorbike.PriceList = new PriceList { HourlyRate = motorbikeDto.HourlyRate, DailyRate = motorbikeDto.DailyRate };
            if (motorbikeDto.formFile != null)
            {
                motorbike.ImageUrl = await fileService.SaveImage(motorbikeDto.formFile, "Motorbike", cancellationToken);
            }
            return mapper.Map<MotorbikeDto>(await motorbikeRepository.Create(motorbike));
        }

        public async Task<bool> DeleteMotorbike(int motorbikeId, CancellationToken cancellationToken = default)
        {
            Motorbike? motorbike = await motorbikeRepository.GetById(motorbikeId, cancellationToken);
            if (motorbike == null)
                throw new NotFoundException("MotorBike not found");
            motorbikeValidator.ValidateForDelete(motorbike);
            await motorbikeRepository.Delete(motorbike, cancellationToken);
            fileService.DeleteFile(motorbike.ImageUrl);
            return true;
        }

        public async Task<PaginatedDataDto<MotorbikeListDto>> GetMotorbikesByFilter(MotorbikeFilterDto motorbikeFilterDto, CancellationToken cancellationToken = default)
        {
            var (data, total) = await motorbikeRepository.GetFilterData(motorbikeFilterDto.CategoryId, motorbikeFilterDto.Brand, motorbikeFilterDto.Status, motorbikeFilterDto.PageNumber, motorbikeFilterDto.PageSize, cancellationToken);
            List<Motorbike> motorbikes = data.ToList();
            List<MotorbikeListDto> motorbikeListDto = new List<MotorbikeListDto>();
            for (int i = 0; i < motorbikes.Count; i++)
            {
                motorbikeListDto.Add(mapper.Map<MotorbikeListDto>(motorbikes[i]));
            }
            return new PaginatedDataDto<MotorbikeListDto>(motorbikeListDto, total);
        }

        public async Task<MotorbikeDto> GetMotorbikeById(int id, CancellationToken cancellationToken = default)
        {
            return mapper.Map<MotorbikeDto>(await motorbikeRepository.GetByIdWithIncludes(id, cancellationToken));
        }

        public async Task<MotorbikeDto> UpdateMotorbike(MotorbikeDto motorbikeDto, CancellationToken cancellationToken = default)
        {
            await motorbikeValidator.ValidateForUpdate(motorbikeDto, cancellationToken);
            Motorbike motorbike = await motorbikeRepository.GetById(motorbikeDto.MotorbikeId, cancellationToken);
            if (motorbike == null)
                throw new Exception("Motorbike not found");
            motorbike.PriceList.DailyRate = motorbikeDto.DailyRate;
            motorbike.PriceList.HourlyRate = motorbikeDto.HourlyRate;
            if (motorbikeDto.formFile != null)
            {
                if (motorbike.ImageUrl != null)
                    if (!fileService.DeleteFile(motorbike.ImageUrl)) throw new Exception("Loi");
                motorbike.ImageUrl = await fileService.SaveImage(motorbikeDto.formFile, "Motorbike", cancellationToken);
            }
            motorbikeDto.ImageUrl = motorbike.ImageUrl;
            await motorbikeRepository.Update(mapper.Map(motorbikeDto, motorbike), cancellationToken);
            return mapper.Map<MotorbikeDto>(motorbike);
        }
        public async Task<MotorbikeIndexDto> GetMotorbikeFilterOptions(CancellationToken cancellationToken = default)
        {
            return new MotorbikeIndexDto
            {
                CategoriesDto = mapper.Map<IEnumerable<CategoryDto>>(await categoryRepository.GetCategoriesNoTracking(cancellationToken)),
                Brands = await motorbikeRepository.GetDistinctBrands(cancellationToken)
            };
        }
    }
}