using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IExternalServices.Storage;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Services.UserServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper mapper;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IEmployeeValidator employeeValidator;
        private readonly IFileService fileService;
        private readonly UserManager<UserCredentials> userManager;
        private readonly IHttpContextAccessor http;
        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository, IEmployeeValidator employeeValidator, IFileService fileService, UserManager<UserCredentials> userManager, IHttpContextAccessor http)
        {
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
            this.employeeValidator = employeeValidator;
            this.fileService = fileService;
            this.userManager = userManager;
            this.http = http;
        }
        public async Task<EmployeeDto> CreateEmployee(EmployeeCreateDto employeeCreateDto, CancellationToken cancellation = default)
        {
            employeeValidator.ValidatorForCreate(employeeCreateDto);
            Employee employee = mapper.Map<Employee>(employeeCreateDto);
            if (employeeCreateDto.FormFile != null)
                employee.Avatar = await fileService.SaveImage(employeeCreateDto.FormFile, "Employee", cancellation);
            await employeeRepository.Create(employee, cancellation);
            return mapper.Map<EmployeeDto>(employee);
        }
        public async Task<EmployeeDto> GetEmployeeById(int id, CancellationToken cancellation = default)
        {
            var result = mapper.Map<EmployeeDto>(await employeeRepository.GetByIdWithIncludes(id, cancellation));
            return result;
        }
        public async Task<EmployeeDto> UpdateEmployee(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellation = default)
        {
            employeeValidator.ValidatorForUpdate(employeeUpdateDto, cancellation);
            Employee employee = await employeeRepository.GetByIdWithIncludes(employeeUpdateDto.EmployeeId, cancellation);
            if (employeeUpdateDto.FormFile != null)
            {
                if (employee.Avatar != null)
                    fileService.DeleteFile(employee.Avatar);
                employee.Avatar = await fileService.SaveImage(employeeUpdateDto.FormFile, "Employee", cancellation);
            }
            employee = mapper.Map(employeeUpdateDto, employee);
            await employeeRepository.Update(employee, cancellation);
            return mapper.Map<EmployeeDto>(employee);
        }
        public async Task<bool> DeleteEmployee(int employeeId, CancellationToken cancellation = default)
        {
            Employee employee = await employeeRepository.GetByIdWithIncludes(employeeId, cancellation);
            if (employee == null)
                throw new NotFoundException($"Employee with id {employeeId} not found");
            if (employee.Avatar != null)
                fileService.DeleteFile(employee.Avatar);
            if (employee.UserCredentials != null)
            {
                IdentityResult result = await userManager.DeleteAsync(employee.UserCredentials);
                if (!result.Succeeded)
                    throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }
            await employeeRepository.Delete(employee, cancellation);
            return true;
        }
        public async Task<PaginatedDataDto<EmployeeListDto>> GetEmployeeByFilter(EmployeeFilterDto filter, CancellationToken cancellation = default)
        {
            string? employeeId = http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            (IEnumerable<Employee> data, int totalCount) = await employeeRepository.GetFilterData(employeeId == null ? null : int.Parse(employeeId), filter.Search, filter.PageNumber, filter.PageSize, filter.RoleId, filter.Status, cancellation);
            return new PaginatedDataDto<EmployeeListDto>(mapper.Map<IEnumerable<EmployeeListDto>>(data), totalCount);
        }
        public async Task<bool> DeleteAvatar(int employeeId, CancellationToken cancellation = default)
        {
            Employee employee = await employeeRepository.GetByIdWithIncludes(employeeId, cancellation);
            if(employee == null)
                throw new NotFoundException($"Employee with id {employeeId} not found");
            if (employee.Avatar == null)
                throw new NotFoundException($"Avatar for employee with id {employeeId} not found");
            fileService.DeleteFile(employee.Avatar);
            employee.Avatar = null;
            await employeeRepository.Update(employee, cancellation);
            return true;
        }
    }
}