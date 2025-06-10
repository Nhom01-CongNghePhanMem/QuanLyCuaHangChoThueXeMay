using AutoMapper;
using MotorbikeRental.Application.DTOs.Customers;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region MapRole
            CreateMap<Roles, RoleDto>();
            #endregion

            #region MapCategory
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            #endregion

            #region MapMotorbike
            CreateMap<Motorbike, MotorbikeDto>()
                .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.HourlyRate,
                opt => opt.MapFrom(src => src.PriceList.HourlyRate))
                .ForMember(dest => dest.DailyRate,
                opt => opt.MapFrom(src => src.PriceList.DailyRate));
            CreateMap<MotorbikeDto, Motorbike>();
            CreateMap<Motorbike, MotorbikeListDto>()
                .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.HourlyRate,
                opt => opt.MapFrom(src => src.PriceList.HourlyRate))
                .ForMember(dest => dest.DailyRate,
                opt => opt.MapFrom(src => src.PriceList.DailyRate));
            #endregion

            #region MapCustomer
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.RentalCount,
                opt => opt.MapFrom(src => src.RentalContracts.Count));
            CreateMap<CustomerDto, Customer>();
            #endregion
        }
    }
}