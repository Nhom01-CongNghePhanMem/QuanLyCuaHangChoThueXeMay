using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MotorbikeRental.Core.Entities.Business.User;
using MotorbikeRental.Core.Entities.General.User;
using MotorbikeRental.Core.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Core.Interfaces.IServices.IUserServices;

namespace MotorbikeRental.Core.Services.UserServices
{
    public class RoleService : IRoleService
    {
        private readonly IMapper mapper;
        private readonly IRoleRepository roleRepository;
        public RoleService(IMapper mapper, IRoleRepository roleRepository)
        {
            this.mapper = mapper;
            this.roleRepository = roleRepository;
        }
        public async Task<IEnumerable<RoleViewModel>> GetAllRoles()
        {
            IEnumerable<Roles> roles = await roleRepository.GetAll();
            return mapper.Map<IEnumerable<RoleViewModel>>(roles);
        }
        public async Task<Roles> GetRoleById(int id)
        {
            return await roleRepository.GetById(id);
        }
    }
}