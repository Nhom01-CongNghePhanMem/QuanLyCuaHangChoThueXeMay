using AutoMapper;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;

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
        public async Task<IEnumerable<RoleDto>> GetAllRoles(CancellationToken cancellationToken = default)
        {
            IEnumerable<Roles> roles = await roleRepository.GetAll(cancellationToken);
            return mapper.Map<IEnumerable<RoleDto>>(roles);
        }
        public async Task<Roles> GetRoleById(int id, CancellationToken cancellationToken = default)
        {
            return await roleRepository.GetById(id, cancellationToken);
        }
    }
}