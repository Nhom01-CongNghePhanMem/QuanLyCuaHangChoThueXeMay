using AutoMapper;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;

namespace MotorbikeRental.Application.Services.UserServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper mapper;
        private readonly IEmployeeRepository employeeRepository;
        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository)
        {
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
        }

        public Task<EmployeeDto> CreateEmployee(EmployeeCreateDto employeeCreateDto, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEmployee(int id, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeDto> GetEmployeeById(int id, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeDto> UpdateEmployee(EmployeeUpdateDto employeeUpdateDto, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }
    }
}