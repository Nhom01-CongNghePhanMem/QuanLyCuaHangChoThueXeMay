using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using System.Threading.Tasks;

namespace MotorbikeRental.Infrastructure.Data.Repositories.PricingRepositories
{
    public class DiscountRepository : BaseRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<(IEnumerable<Discount>, int)> GetFilterData(string? search, int pageNumber, int pageSize, DateTime? filterStartDate, DateTime? filterEndDate,bool? isActive, CancellationToken cancellationToken = default)
        {
            IQueryable<Discount> queryable = dbContext.Discounts.AsNoTracking()
                .Include(d => d.Categories)
                .ThenInclude(dc => dc.Category)
                .OrderByDescending(d => d.DiscountId);
            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                queryable = queryable.Where(d => d.Name.Contains(lowerSearch));
            }
            if (filterStartDate != null)
                queryable = queryable.Where(d => d.StartDate > filterStartDate);
            if (filterEndDate != null)
                queryable = queryable.Where(d => d.StartDate < filterEndDate);
            if(isActive != null)
                queryable = queryable.Where(d => d.IsActive == isActive);
            int totalCount = queryable.Count();
            queryable = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return (await queryable.ToListAsync(cancellationToken), totalCount);
        }
        public async Task<Discount?> GetDiscountById(int id, bool isTracking, CancellationToken cancellationToken = default)
        {
            IQueryable<Discount> queryable = dbContext.Discounts.Where(d => d.DiscountId == id)
                .Include(d => d.Categories)
                .ThenInclude(dc => dc.Category);
            if (!isTracking)
                queryable = queryable.AsNoTracking();
            return await queryable.FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<IEnumerable<Discount>> GetExpiredDiscounts(CancellationToken cancellationToken = default)
        {
            return await dbContext.Discounts.Where(d => d.IsActive && d.EndDate < DateTime.UtcNow).ToListAsync(cancellationToken);
        }
    }
}