using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MotorbikeRental.Application.DTOs.AuthenticDto;
using MotorbikeRental.Application.Interface.IServices.IAuthServices;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;

namespace MotorbikeRental.Application.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserCredentials> userManager;
        private readonly IUserCredentialsRepository userCredentialsRepository;
        private readonly IMapper mapper;
        public AuthService(UserManager<UserCredentials> userManager, IUserCredentialsRepository userCredentialsRepository, IMapper mapper)
        {
            this.userManager = userManager;
            this.userCredentialsRepository = userCredentialsRepository; 
            this.mapper = mapper;
        }
        public async Task<UserCredentialsDto> Login(LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            UserCredentials? userCredentials = await userCredentialsRepository.GetByUserNameInCludes(loginDto.UserName, cancellationToken);
            if (userCredentials != null && await userManager.CheckPasswordAsync(userCredentials, loginDto.Password))
                return mapper.Map<UserCredentialsDto>(userCredentials);
            return null;
        }   
    }
}
