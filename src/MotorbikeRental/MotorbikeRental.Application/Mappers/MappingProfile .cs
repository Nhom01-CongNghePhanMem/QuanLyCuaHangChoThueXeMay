using AutoMapper;
using MotorbikeRental.Domain.DTOs.User;
using MotorbikeRental.Domain.DTOs.Vehicles;
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
            CreateMap<Motorbike, MotorbikeDto>();
            CreateMap<MotorbikeDto, Motorbike>();
            CreateMap<Motorbike, MotorbikeDto>();
            #endregion
        }
    }
}