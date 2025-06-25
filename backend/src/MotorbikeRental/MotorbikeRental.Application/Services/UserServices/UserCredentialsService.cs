using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;

namespace MotorbikeRental.Application.Services.UserServices
{
    public class UserCredentialsService : IUserCredentialsService
    {
        private readonly IUserCredentialsValidator userCredentialsValidator;
        private readonly IUserCredentialsRepository userCredentialsRepository;
        private readonly IMapper mapper;
        private readonly UserManager<UserCredentials> userManager;
        public UserCredentialsService(IUserCredentialsValidator userCredentialsValidator, IMapper mapper, UserManager<UserCredentials> userManager, IUserCredentialsRepository userCredentialsRepository)
        {
            this.userCredentialsValidator = userCredentialsValidator;
            this.mapper = mapper;
            this.userManager = userManager;
            this.userCredentialsRepository = userCredentialsRepository;
        }
        public async Task<EmployeeDto> CreateUserCredentials(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default)
        {
            if (employeeCreateDto.UserName == null)
                employeeCreateDto.UserName = employeeCreateDto.Email;
            await userCredentialsValidator.ValidatorForCreate(employeeCreateDto);
            UserCredentials userCredentials = mapper.Map<UserCredentials>(employeeCreateDto);
            userCredentials.Employee = mapper.Map<Employee>(employeeCreateDto);
            Console.WriteLine("Username: " + userCredentials.UserName);
            IdentityResult identityResult = await userManager.CreateAsync(userCredentials, employeeCreateDto.Password);
            return identityResult.Succeeded
                ? mapper.Map<EmployeeDto>(userCredentials)
                : throw new Exception(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
        }
    }
}