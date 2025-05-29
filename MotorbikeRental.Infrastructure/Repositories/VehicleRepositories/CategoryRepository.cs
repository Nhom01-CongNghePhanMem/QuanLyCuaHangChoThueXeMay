using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data;

namespace MotorbikeRental.Infrastructure.Repositories.VehicleRepositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<bool> CategoryNameExists(string categoryName)
        {
            return await dbContext.Set<Category>().AsNoTracking().AnyAsync(c => c.CategoryName == categoryName);
        }
        public async Task<bool> CategoryIdExists(int categoryId)
        {
            return await dbContext.Set<Category>().AsNoTracking().AnyAsync(c => c.CategoryId == categoryId);
        }
    }
}