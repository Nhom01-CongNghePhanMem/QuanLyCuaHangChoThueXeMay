using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MotorbikeRental.Application.DTOs.AuthenticDto;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IExternalServices.Storage;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Services.UserServices
{
    public class UserCredentialsService : IUserCredentialsService
    {
        private readonly IUserCredentialsValidator userCredentialsValidator;
        private readonly IMapper mapper;
        private readonly UserManager<UserCredentials> userManager;
        private readonly RoleManager<Roles> roleManager;
        private readonly IFileService fileService;
        private readonly IUserCredentialsRepository userCredentialsRepository;
        public UserCredentialsService(IUserCredentialsValidator userCredentialsValidator, IMapper mapper, UserManager<UserCredentials> userManager, IFileService fileService, RoleManager<Roles> roleManager, IUserCredentialsRepository userCredentialsRepository)
        {
            this.userCredentialsValidator = userCredentialsValidator;
            this.mapper = mapper;
            this.userManager = userManager;
            this.fileService = fileService;
            this.roleManager = roleManager;
            this.userCredentialsRepository = userCredentialsRepository;
        }
        public async Task<bool> CreateUserCredentials(UserCredentialsCreateDto userCredentialsCreateDto, CancellationToken cancellationToken = default)
        {
            await userCredentialsValidator.ValidatorForCreate(userCredentialsCreateDto, cancellationToken);
            Roles? roles = await roleManager.FindByIdAsync(userCredentialsCreateDto.RoleId.ToString());
            if (roles == null)
                throw new NotFoundException($"Role with id {userCredentialsCreateDto.RoleId} not found");
            UserCredentials userCredentials = mapper.Map<UserCredentials>(userCredentialsCreateDto);
            if (userCredentialsCreateDto.UserName == null)
                userCredentials.UserName = userCredentialsCreateDto.Email;
            IdentityResult identityResult = await userManager.CreateAsync(userCredentials, userCredentialsCreateDto.Password);
            if (!identityResult.Succeeded)
                throw new ValidatorException(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            await userManager.AddToRoleAsync(userCredentials, roles.Name);
            return true;
        }
        public async Task<bool> ResetEmail(ResetEmailDto resetEmail, CancellationToken cancellationToken = default)
        {
            UserCredentials userCredentials = await userCredentialsRepository.GetByEmployeeId(resetEmail.EmployeeId, cancellationToken);
            if (userCredentials == null)
                throw new NotFoundException($"UserCredentials with id {resetEmail.EmployeeId} not found");
            if (userCredentials.Email == resetEmail.Email)
                throw new ValidatorException("New email cannot be the same as the current email");
            if (userCredentials.Email == userCredentials.UserName)
                await userManager.SetUserNameAsync(userCredentials, resetEmail.Email);
            IdentityResult identityResult = await userManager.SetEmailAsync(userCredentials, resetEmail.Email);
            if (!identityResult.Succeeded)
                throw new ValidatorException(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            return true;
        }
        public async Task<bool> ResetPhoneNumber(ResetPhoneNumberDto resetPhoneNumber, CancellationToken cancellationToken = default)
        {
            UserCredentials userCredentials = userCredentialsRepository.GetByEmployeeId(resetPhoneNumber.EmployeeId, cancellationToken).Result;
            if (userCredentials == null)
                throw new NotFoundException($"UserCredentials with id {resetPhoneNumber.EmployeeId} not found");
            if (userCredentials.PhoneNumber == resetPhoneNumber.PhoneNumber)
                throw new ValidatorException("New phone number cannot be the same as the current phone number");
            await userManager.SetPhoneNumberAsync(userCredentials, resetPhoneNumber.PhoneNumber);
            return true;
        }
        public async Task<bool> ResetUserName(ResetUserNameDto resetUserName, CancellationToken cancellationToken = default)
        {
            UserCredentials userCredentials = await userCredentialsRepository.GetByEmployeeId(resetUserName.EmployeeId, cancellationToken);
            if (userCredentials == null)
                throw new NotFoundException($"UserCredentials with id {resetUserName.EmployeeId} not found");
            if (userCredentials.UserName == resetUserName.UserName)
                throw new ValidatorException("New username cannot be the same as the current username");
            IdentityResult identityResult = await userManager.SetUserNameAsync(userCredentials, resetUserName.UserName);
            if (!identityResult.Succeeded)
                throw new ValidatorException(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            return true;
        }
        public async Task<bool> ResetPasswordByAdmin(ResetPasswordDto resetPassword, CancellationToken cancellationToken = default)
        {
            UserCredentials userCredentials = await userCredentialsRepository.GetByEmployeeId(resetPassword.EmployeeId, cancellationToken);
            if (userCredentials == null)
                throw new NotFoundException($"UserCredentials with id {resetPassword.EmployeeId} not found");
            string token = await userManager.GeneratePasswordResetTokenAsync(userCredentials);
            IdentityResult identityResult = await userManager.ResetPasswordAsync(userCredentials,token, resetPassword.Password);
            if (!identityResult.Succeeded)
                throw new ValidatorException(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            return true;
        }
    }
}