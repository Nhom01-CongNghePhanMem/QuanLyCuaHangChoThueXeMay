using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Entities.General.Contract;
using MotorbikeRental.Core.Entities.General.Customers;
using MotorbikeRental.Core.Entities.General.User;
using MotorbikeRental.Core.Entities.General.Vehicles;

namespace MotorbikeRental.Infrastructure.Data
{
    public class MotorbikeRentalDbContextConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            ConfigureRoles(modelBuilder);
            ConfigureEmployee(modelBuilder);
            ConfigurationCategory(modelBuilder);
            ConfigurationMotorbike(modelBuilder);
            ConfigurationMaintenanceRecord(modelBuilder);
            ConfigurationCustomer(modelBuilder);
            ConfigurationRentalContract(modelBuilder);
        }

        public static void ConfigureRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>(e =>
           {
               e.ToTable("Roles");
               e.Property(e => e.RoleId).ValueGeneratedOnAdd();
               e.HasKey(e => e.RoleId);
               e.HasMany(e => e.Employees)
                    .WithOne(e => e.Roles);
           });
        }

        public static void ConfigureEmployee(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");
                entity.HasKey(employcee => employcee.UserId);
                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Status)
                    .HasConversion<string>();
                entity.HasOne(e => e.Roles)
                    .WithMany(e => e.Employees)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(e => e.MaintenanceRecords)
                    .WithOne(e => e.Employee);
            });
        }

        public static void ConfigurationCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryId).ValueGeneratedOnAdd();
                entity.HasMany(e => e.Motorbikes)
                    .WithOne(e => e.Category);
            });
        }

        public static void ConfigurationMotorbike(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Motorbike>(entity =>
            {
                entity.ToTable("Motorbike");
                entity.HasKey(e => e.MotorbikeId);
                entity.Property(e => e.MotorbikeId).ValueGeneratedOnAdd();
                entity.HasOne(e => e.Category)
                    .WithMany(e => e.Motorbikes)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(e => e.MaintenanceRecords)
                    .WithOne(e => e.Motorbike);
                entity.Property(e => e.Status)
                    .HasConversion<string>();
                entity.Property(e => e.MotorbikeConditionStatus)
                    .HasConversion<string>();
            });
        }
        public static void ConfigurationMaintenanceRecord(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaintenanceRecord>(entity =>
            {
                entity.ToTable("MaintenanceRecord");
                entity.HasKey(e => e.MaintenanceRecordId);
                entity.Property(e => e.MaintenanceRecordId).ValueGeneratedOnAdd();
                entity.HasOne(e => e.Motorbike)
                    .WithMany(e => e.MaintenanceRecords)
                    .HasForeignKey(e => e.MotorbikeId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Employee)
                    .WithMany(e => e.MaintenanceRecords)
                    .HasForeignKey(e => e.EmployeeId);
                entity.Property(e => e.MaintenanceTypeStatus)
                    .HasConversion<string>();
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
            });
        }
        public static void ConfigurationCustomer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.CustomerId).ValueGeneratedOnAdd();
                entity.Property(e => e.CreateAt)
                    .HasDefaultValueSql("GETDATE()");
                entity.HasMany(e => e.RentalContracts)
                    .WithOne(e => e.Customer);
            });
        }
        public static void ConfigurationRentalContract(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RentalContract>(entity =>
            {
                entity.ToTable("RentalContract");
                entity.HasKey(e => e.ContractId);
                entity.Property(e => e.ContractId).ValueGeneratedOnAdd();
                entity.HasOne(e => e.Customer)
                    .WithMany(e => e.RentalContracts)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.Property(e => e.RentalDate)
                    .HasDefaultValueSql("GETDATE()");
                entity.HasOne(e => e.Motorbike)
                    .WithMany(e => e.RentalContracts)
                    .HasForeignKey(e => e.MotorbikeId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.Property(e => e.RentalContractStatus)
                    .HasConversion<string>();
                entity.Property(e => e.RentalTypeStatus)
                    .HasConversion<string>();
            });
        }
    }
}