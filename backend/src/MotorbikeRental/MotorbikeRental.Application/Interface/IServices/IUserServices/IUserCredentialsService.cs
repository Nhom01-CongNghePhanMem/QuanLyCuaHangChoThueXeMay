using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Application.Interface.IServices.IUserServices
{
    public interface IUserCredentialsService
    {
        Task<EmployeeDto> CreateUserCredentials(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default);
    }
}