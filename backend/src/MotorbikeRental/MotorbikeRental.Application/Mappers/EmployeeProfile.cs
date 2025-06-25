using AutoMapper;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Application.Mappers
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<UserCredentials, EmployeeDto>()
                .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.Employee.EmployeeId))
                .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(dest => dest.DateOfBirth,
                opt => opt.MapFrom(src => src.Employee.DateOfBirth))
                .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Employee.Address))
                .ForMember(dest => dest.Avatar,
                opt => opt.MapFrom(src => src.Employee.Avatar))
                .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom(src => src.Roles.Name))
                .ForMember(dest => dest.StartDate,
                opt => opt.MapFrom(src => src.Employee.StartDate))
                .ForMember(dest => dest.Salary,
                opt => opt.MapFrom(src => src.Employee.Salary))
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Employee.Status));
            CreateMap<EmployeeCreateDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();
        }
    }
}