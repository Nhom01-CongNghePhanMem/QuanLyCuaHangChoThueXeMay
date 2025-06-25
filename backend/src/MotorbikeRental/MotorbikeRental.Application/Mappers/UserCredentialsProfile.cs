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
        }
    }
}