using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Entities.General.User;
using MotorbikeRental.Core.Entities.General.Vehicles;

namespace MotorbikeRental.Infrastructure.Data
{
    public class MotorbikeRentalDbContext : DbContext
    {
        public MotorbikeRentalDbContext(DbContextOptions<MotorbikeRentalDbContext> options) : base(options){}
        #region DbSet Section
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Motorbike> Motorbikes { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            MotorbikeRentalDbContextConfigurations.Configure(modelBuilder);
        }
    }
}