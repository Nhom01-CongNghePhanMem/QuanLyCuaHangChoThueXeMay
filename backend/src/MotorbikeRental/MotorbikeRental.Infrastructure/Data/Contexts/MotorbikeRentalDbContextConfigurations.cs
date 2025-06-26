using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Infrastructure.Data.Contexts
{
    public class MotorbikeRentalDbContextConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            ConfigurationRoles(modelBuilder);
            ConfigureEmployee(modelBuilder);
            ConfigurationCategory(modelBuilder);
            ConfigurationMotorbike(modelBuilder);
            ConfigurationMaintenanceRecord(modelBuilder);
            ConfigurationCustomer(modelBuilder);
            ConfigurationRentalContract(modelBuilder);
            ConfigurationDiscount(modelBuilder);
            ConfigurationIncident(modelBuilder);
            ConfigurationIncidentImage(modelBuilder);
            ConfigurationPayment(modelBuilder);
            ConfigurationPriceList(modelBuilder);
            ConfigurationUserCredentials(modelBuilder);
        }

        public static void ConfigurationRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>(e =>
           {
               e.ToTable("Roles");
               e.Property(e => e.Id).ValueGeneratedOnAdd();
               e.HasKey(e => e.Id);
               e.HasMany(e => e.UserCredentials)
                    .WithOne(e => e.Roles);
           });
        }

        public static void ConfigureEmployee(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.EmployeeId)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Status)
                    .HasConversion<string>();
                entity.HasOne(e => e.UserCredentials)
                    .WithOne(e => e.Employee);
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
                entity.Property(e => e.Gender)
                    .HasConversion<string>();
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
                entity.HasOne(e => e.Employee)
                    .WithMany(e => e.RentalContracts)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.RentalTypeStatus)
                    .HasConversion<string>();
            });
        }
        public static void ConfigurationDiscount(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discount");
                entity.Property(e => e.DiscountId).ValueGeneratedOnAdd();
                entity.HasKey(e => e.DiscountId);
                entity.HasOne(e => e.Category)
                    .WithOne(e => e.Discount)
                    .HasForeignKey<Discount>(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.StartDate)
                    .HasDefaultValueSql("GETDATE()");
            });
        }
        public static void ConfigurationIncident(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Incident>(entity =>
            {
                entity.ToTable("Incident");
                entity.HasKey(e => e.IncidentId);
                entity.Property(e => e.IncidentId).ValueGeneratedOnAdd();
                entity.HasOne(e => e.RentalContract)
                    .WithOne(e => e.Incident)
                    .HasForeignKey<Incident>(e => e.ContractId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Motorbike)
                    .WithMany(e => e.Incidents)
                    .HasForeignKey(e => e.MotorbikeId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.Property(e => e.Severity)
                    .HasConversion<string>();
                entity.HasOne(e => e.ReportedByEmployee)
                    .WithMany(e => e.Incidents)
                    .HasForeignKey(e => e.ReportedByEmployeeId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(e => e.Images)
                    .WithOne(e => e.Incident)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        public static void ConfigurationIncidentImage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IncidentImage>(entity =>
            {
                entity.ToTable("IncidentImage");
                entity.HasKey(e => e.ImageId);
                entity.Property(e => e.ImageId).ValueGeneratedOnAdd();
                entity.HasOne(e => e.Incident)
                    .WithMany(e => e.Images)
                    .HasForeignKey(e => e.IncidentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        public static void ConfigurationPayment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");
                entity.HasKey(e => e.PaymentId);
                entity.Property(e => e.PaymentId).ValueGeneratedOnAdd();
                entity.HasOne(e => e.RentalContract)
                    .WithOne(e => e.Payments)
                    .HasForeignKey<Payment>(e => e.ContractId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.PaymentStatus)
                    .HasConversion<string>();
                entity.HasOne(e => e.Discount)
                    .WithMany()
                    .HasForeignKey(e => e.DiscountId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
        public static void ConfigurationPriceList(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PriceList>(entity =>
            {
                entity.ToTable("PriceList");
                entity.HasKey(e => e.PriceListId);
                entity.Property(e => e.PriceListId).ValueGeneratedOnAdd();
                entity.HasOne(e => e.Motorbike)
                    .WithOne(e => e.PriceList)
                    .HasForeignKey<PriceList>(e => e.MotorbikeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        public static void ConfigurationUserCredentials(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCredentials>(entity =>
            {
                entity.ToTable("UserCredentials");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne(e => e.Employee)
                    .WithOne(e => e.UserCredentials)
                    .HasForeignKey<UserCredentials>(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Roles)
                    .WithMany(e => e.UserCredentials)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }

}