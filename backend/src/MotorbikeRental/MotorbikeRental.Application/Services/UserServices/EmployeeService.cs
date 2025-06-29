using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IExternalServices.Storage;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;

namespace MotorbikeRental.Application.Services.UserServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper mapper;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IEmployeeValidator employeeValidator;
        private readonly IFileService fileService;
        private readonly UserManager<UserCredentials> userManager;
        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository, IEmployeeValidator employeeValidator, IFileService fileService, UserManager<UserCredentials> userManager)
        {
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
            this.employeeValidator = employeeValidator;
            this.fileService = fileService;
            this.userManager = userManager;
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
            return mapper.Map<EmployeeDto>(await employeeRepository.GetByIdWithIncludes(id, cancellation));
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
            if(employee == null)
                throw new NotFoundException($"Employee with id {employeeId} not found");
            if(employee.Avatar != null)
                fileService.DeleteFile(employee.Avatar);
            if(employee.UserCredentials != null)
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
            (IEnumerable<Employee> data, int totalCount) = await employeeRepository.GetFilterData(filter.Search, filter.PageNumber, filter.PageSize, filter.RoleId, filter.Status, cancellation);
            List<Employee> employees = data.ToList();
            List<EmployeeListDto> employeesList = new List<EmployeeListDto>();
            for(int i = 0; i < employees.Count; i++)
            {
                employeesList.Add(mapper.Map<EmployeeListDto>(employees[i]));
            }
            return new PaginatedDataDto<EmployeeListDto>(employeesList, totalCount);
        }
    }
}