using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.Contract;
using MotorbikeRental.Core.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Core.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Repositories.ContractRepositories;
using MotorbikeRental.Infrastructure.Repositories.UserRepositories;
using MotorbikeRental.Infrastructure.Repositories.VehicleRepositories;

namespace MotorbikeRental.View.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            #region RegisterRepositories
            //VehicleRepository
            services.AddScoped<IMotorbikeRepository, MotorbikeRepository>();
            services.AddScoped<IMaintenanceRecordRepository, MaintenanceRecordRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            //UserRepository
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            //ContractRepository
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IRentalContractRepository, RentalContractRepository>();
            #endregion
            return services;
        }
    }
}