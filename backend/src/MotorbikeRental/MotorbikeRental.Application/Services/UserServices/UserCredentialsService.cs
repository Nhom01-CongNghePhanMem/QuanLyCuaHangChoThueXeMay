using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        public async Task<EmployeeDto> CreateUserCredentials(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default)
        {
            Roles? roles = await roleManager.FindByIdAsync(employeeCreateDto.RoleId.ToString());
            if (roles == null)
                throw new NotFoundException($"Role with id {employeeCreateDto.RoleId} not found");
            if (employeeCreateDto.UserName == null)
                employeeCreateDto.UserName = employeeCreateDto.Email;
            await userCredentialsValidator.ValidatorForCreate(employeeCreateDto, cancellationToken);
            UserCredentials userCredentials = mapper.Map<UserCredentials>(employeeCreateDto);
            userCredentials.Employee = mapper.Map<Employee>(employeeCreateDto);
            IdentityResult identityResult = await userManager.CreateAsync(userCredentials, employeeCreateDto.Password);
            if (identityResult.Succeeded)
            {
                if (employeeCreateDto.FormFile != null)
                    await fileService.SaveImage(employeeCreateDto.FormFile, "Employee", cancellationToken);
                await userManager.AddToRoleAsync(userCredentials, roles.Name);
            }
            else
            {
                throw new ValidatorException(string.Join("; ", identityResult.Errors.Select(e => e.Description)));
            }
            return mapper.Map<EmployeeDto>(userCredentials);
        }
        public async Task<EmployeeDto> UpdateUserCredentials(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default)
        {
            UserCredentials? userCredentials = await userCredentialsRepository.GetByIdWithIncludes(employeeUpdateDto.EmployeeId);
            if (userCredentials == null) 
                throw new NotFoundException($"Employee with id {employeeUpdateDto.EmployeeId} not found");
            Roles? roles = await roleManager.FindByIdAsync(employeeUpdateDto.RoleId.ToString());
            if (roles == null)
                throw new NotFoundException($"Role with id {employeeUpdateDto.RoleId} not found");
            await userCredentialsValidator.ValidatorForUpdate(employeeUpdateDto, cancellationToken);
            if (userCredentials.Employee.Avatar != null)
            {
                if (!fileService.DeleteFile(userCredentials.Employee.Avatar))
                    throw new Exception("System is experiencing issues. Please try again later");
            }
            mapper.Map(employeeUpdateDto, userCredentials.Employee);
            mapper.Map(employeeUpdateDto, userCredentials);
            if(employeeUpdateDto.Password != null)
            {
            }
            return null;
        }
    }
}