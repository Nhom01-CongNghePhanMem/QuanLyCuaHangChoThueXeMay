using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MotorbikeRental.Core.Entities.Business.User;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Entities.General.User;

namespace MotorbikeRental.Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region MapRole
            CreateMap<Roles, RoleViewModel>();
            #endregion

            #region MapCategory
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();
            #endregion

            #region MapMotorbike
            CreateMap<Motorbike, MotorbikeViewModel>(); 
            CreateMap<MotorbikeViewModel, Motorbike>();
            CreateMap<Motorbike, MotorbikeListViewModel>();
            #endregion
        }
    }
}