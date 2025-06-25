using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.API.Extensions
{
    public static class SecurityExtension
    {
        public static IServiceCollection RegisterSecurityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<UserCredentials, Roles>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<MotorbikeRentalDbContext>()
            .AddDefaultTokenProviders();
            return services;
        }
    }
}