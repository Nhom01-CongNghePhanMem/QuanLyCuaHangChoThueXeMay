using AutoMapper;
using MotorbikeRental.Domain.DTOs.User;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Domain.Interfaces.IServices.IUserServices;

namespace MotorbikeRental.Application.Services.UserServices
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
        public async Task<IEnumerable<RoleDto>> GetAllRoles()
        {
            IEnumerable<Roles> roles = await roleRepository.GetAll();
            return mapper.Map<IEnumerable<RoleDto>>(roles);
        }
        public async Task<Roles> GetRoleById(int id)
        {
            return await roleRepository.GetById(id);
        }
    }
}