using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.VehicleRepositories
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
        public async Task<Category> GetByIdNoAsTracking(int id)
        {
            return await dbContext.Categories.Where(c => c.CategoryId == id).AsNoTracking().FirstAsync() ?? throw new Exception("Category not found");
        }
        public async Task<IEnumerable<Category>> GetCategoriesNoTracking()
        {
            return await dbContext.Categories.AsNoTracking().ToListAsync();
        }
    }
}