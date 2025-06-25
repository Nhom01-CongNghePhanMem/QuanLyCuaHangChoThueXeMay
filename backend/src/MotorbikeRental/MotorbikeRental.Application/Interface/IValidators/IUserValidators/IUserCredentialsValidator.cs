using Microsoft.AspNetCore.Identity;
using MotorbikeRental.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Interface.IValidators.IUserValidators
{
    public interface IUserCredentialsValidator
    {
        Task<IdentityResult> ValidatorForCreate(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default);
        Task<IdentityResult> ValidatorForUpdate(EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default);
    }
}