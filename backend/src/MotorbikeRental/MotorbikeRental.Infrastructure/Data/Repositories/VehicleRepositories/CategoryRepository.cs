using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using MotorbikeRental.Application.Exceptions;

namespace MotorbikeRental.Infrastructure.Data.Repositories.VehicleRepositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Category> GetByIdNoAsTracking(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Categories.Where(c => c.CategoryId == id).AsNoTracking().FirstAsync(cancellationToken) ?? throw new NotFoundException("Category not found");
        }
        public async Task<IEnumerable<Category>> GetCategoriesNoTracking(CancellationToken cancellationToken = default)
        {
            return await dbContext.Categories.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}