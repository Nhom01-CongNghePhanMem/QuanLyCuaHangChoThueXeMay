using AutoMapper;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IExternalServices.Storage;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;
using MotorbikeRental.Application.Validators.UserValidators;
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
        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository, IEmployeeValidator employeeValidator, IFileService fileService)
        {
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
            this.employeeValidator = employeeValidator;
            this.fileService = fileService;
        }
    }
}