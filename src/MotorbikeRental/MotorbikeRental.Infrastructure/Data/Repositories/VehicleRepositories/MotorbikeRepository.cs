using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.VehicleRepositories
{
    public class MotorbikeRepository : BaseRepository<Motorbike>, IMotorbikeRepository
    {
        public MotorbikeRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Motorbike> GetByIdNoAsTracking(int id)
        {
            return await dbContext.Motorbikes.Where(c => c.MotorbikeId == id).AsNoTracking().FirstOrDefaultAsync() ?? throw new Exception("Category not found");
        }
        public async Task<Motorbike> GetByIdWithIncludes(int id)
        {
            return await dbContext.Motorbikes.Where(m => m.MotorbikeId == id)
                .AsNoTracking()
                .Include(m => m.Category)
                .Include(m => m.PriceList)
                .Include(m => m.Incidents)
                .FirstOrDefaultAsync() ?? throw new Exception("Motorbike not found");
        }
        public async Task<(IEnumerable<Motorbike>, int totalCount)> GetFilterData(int? categoryId, string? brand, MotorbikeStatus? status, int pageNumber, int pageSize)
        {
            IQueryable<Motorbike> queryable = dbContext.Motorbikes
                .Include(m => m.Category)
                .Include(m => m.PriceList)
                .AsQueryable();
            if (categoryId != null)
                queryable = queryable.Where(m => m.CategoryId == categoryId);
            if (status != null)
                queryable = queryable.Where(m => m.Status == status);
            if (!string.IsNullOrWhiteSpace(brand))
                queryable = queryable.Where(m => m.Brand == brand);
            int totalCount = await queryable.CountAsync();
            queryable = queryable.AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return (await queryable.ToListAsync(), totalCount);
        }
        public async Task<IEnumerable<string>> GetDistinctBrands()
        {
            return await dbContext.Motorbikes
                .Where(m => !string.IsNullOrEmpty(m.Brand))
                .Select(m => m.Brand)
                .Distinct()
                .ToListAsync();
        }
    }
}