using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Application.Interface.IServices.IUserServices
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> CreateEmployee(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default);
        Task<EmployeeDto> UpdateEmployee(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteEmployee(int id, CancellationToken cancellationToken = default);
        Task<EmployeeDto> GetEmployeeById(int id, CancellationToken cancellationToken = default);
    }
}