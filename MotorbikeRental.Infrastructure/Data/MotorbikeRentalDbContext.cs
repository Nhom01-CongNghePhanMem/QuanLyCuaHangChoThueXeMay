using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MotorbikeRental.Infrastructure.Data
{
    public class MotorbikeRentalDbContext : DbContext
    {
        public MotorbikeRentalDbContext(DbContextOptions<MotorbikeRentalDbContext> options) : base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            MotorbikeRentalDbContextConfigurations.Configure(modelBuilder);
        }
    }
}