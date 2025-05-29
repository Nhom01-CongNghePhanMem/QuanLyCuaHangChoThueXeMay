using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MotorbikeRental.Core.Entities.Business.User;
using MotorbikeRental.Core.Entities.General.User;
using MotorbikeRental.Core.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Core.Services.UserServices;

namespace MotorbikeRental.UnitTest.Core
{
    public class RoleServiceTests
    {
        private readonly Mock<IRoleRepository> mockRoleRepository;
        private readonly IMapper mapper;
        private readonly RoleService roleService;
        public RoleServiceTests()
        {
            mockRoleRepository = new Mock<IRoleRepository>();
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Roles, RoleViewModel>();
            });
            mapper = config.CreateMapper();
            roleService = new RoleService(mapper, mockRoleRepository.Object);
        }
        [Fact]
        public async Task GetAllRoleTest()
        {
            IEnumerable<Roles> roles = new List<Roles>
            {
                new Roles {RoleId = 1, RoleName = "Admin"},
                new Roles {RoleId = 2, RoleName = "User"}
            };
            mockRoleRepository.Setup(repo => repo.GetAll())
                      .ReturnsAsync(roles);

            IEnumerable<RoleViewModel> result = await roleService.GetAllRoles();
            List<RoleViewModel> list = result.ToList();
            Assert.Equal(2, list.Count);
            Assert.Equal("Admin", list[0].RoleName);
            Assert.Equal("User", list[1].RoleName);
        }
    }
}