using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Core.Entities.General;

namespace MotorbikeRental.Infrastructure.Data
{
    public class MotorbikeRentalDbContextConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>(e =>
            {
                e.ToTable("Roles");
                e.HasKey(roles => roles.RoleId);
            });
            modelBuilder.Entity<Employee>(e =>
            {
                e.ToTable("Employee");
                e.HasKey(employcee => employcee.UserId);
            });
        }
    }
}