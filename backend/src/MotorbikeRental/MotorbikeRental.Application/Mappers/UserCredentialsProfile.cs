using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Application.Mappers
{
    public class UserCredentialsProfile : Profile
    {
        public UserCredentialsProfile()
        {
            CreateMap<EmployeeCreateDto, UserCredentials>()
                .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.RoleId,
                opt => opt.MapFrom(src => src.RoleId));
            CreateMap<EmployeeUpdateDto, UserCredentials>()
                .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.RoleId,
                opt => opt.MapFrom(src => src.RoleId));
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