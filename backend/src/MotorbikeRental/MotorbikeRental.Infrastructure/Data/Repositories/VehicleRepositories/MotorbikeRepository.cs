using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.VehicleRepositories
{
    public class MotorbikeRepository : BaseRepository<Motorbike>, IMotorbikeRepository
    {
        public MotorbikeRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Motorbike> GetByIdNoAsTracking(int id,CancellationToken cancellationToken = default)
        {
            return await dbContext.Motorbikes.Where(c => c.MotorbikeId == id).AsNoTracking().FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Category not found");
        }
        public async Task<Motorbike> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Motorbikes.Where(m => m.MotorbikeId == id)
                .AsNoTracking()
                .Include(m => m.Category)
                .Include(m => m.PriceList)
                .Include(m => m.Incidents)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Motorbike not found");
        }
        public async Task<(IEnumerable<Motorbike>, int totalCount)> GetFilterData(int? categoryId, string? brand, MotorbikeStatus? status, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            IQueryable<Motorbike> queryable = dbContext.Motorbikes
                .OrderBy(m => m.MotorbikeId)
                .Include(m => m.Category)
                .Include(m => m.PriceList)
                .AsQueryable();
            if (categoryId != null)
                queryable = queryable.Where(m => m.CategoryId == categoryId);
            if (status != null)
                queryable = queryable.Where(m => m.Status == status);
            if (!string.IsNullOrWhiteSpace(brand))
                queryable = queryable.Where(m => m.Brand == brand);
            int totalCount = await queryable.CountAsync(cancellationToken);
            queryable = queryable.AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return (await queryable.ToListAsync(cancellationToken), totalCount);
        }
        public async Task<IEnumerable<string>> GetDistinctBrands(CancellationToken cancellationToken = default)
        {
            return await dbContext.Motorbikes
                .Where(m => !string.IsNullOrEmpty(m.Brand))
                .Select(m => m.Brand)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}