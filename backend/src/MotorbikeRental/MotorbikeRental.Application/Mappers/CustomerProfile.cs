using AutoMapper;
using MotorbikeRental.Application.DTOs.Customers;
using MotorbikeRental.Domain.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Mappers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>()
                    .ForMember(dest => dest.RentalCount,
                    opt => opt.MapFrom(src => src.RentalContracts.Count));
            CreateMap<CustomerDto, Customer>();
        }
    }
}
