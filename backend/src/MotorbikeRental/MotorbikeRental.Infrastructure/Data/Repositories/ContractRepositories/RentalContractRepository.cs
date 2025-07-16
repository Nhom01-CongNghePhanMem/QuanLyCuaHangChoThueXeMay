using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Enums.ContractEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using System.Threading.Tasks;

namespace MotorbikeRental.Infrastructure.Data.Repositories.ContractRepositories
{
    public class RentalContractRepository : BaseRepository<RentalContract>, IRentalContractRepository
    {
        public RentalContractRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<RentalContract?> GetContractById(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.RentalContracts
                .AsNoTracking()
                .Where(r => r.ContractId == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<(IEnumerable<RentalContract>, int totalCount)> GetFilterData(string? search, int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, RentalContractStatus? status, CancellationToken cancellation = default)
        {
            IQueryable<RentalContract> queryable = dbContext.RentalContracts
                .AsNoTracking()
                .Include(e => e.Customer)
                .Include(e => e.Motorbike)
                .Include(e => e.Discount)
                .OrderByDescending(r => r.ContractId)
                .AsQueryable();
            if(!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                queryable = queryable.Where(r => r.Customer.FullName.Contains(lowerSearch) || r.Motorbike.MotorbikeName.Contains(lowerSearch) || r.Motorbike.LicensePlate.Contains(lowerSearch) || r.Employee.FullName.Contains(lowerSearch));
            }
            if (fromDate.HasValue)
                queryable = queryable.Where(r => r.RentalDate >= fromDate.Value);
            if (toDate.HasValue)
                queryable = queryable.Where(r => r.RentalDate <= toDate.Value);
            if (status.HasValue)
                queryable = queryable.Where(r => r.RentalContractStatus == status.Value);
            int totalCount = queryable.Count();
            queryable = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return (await queryable.ToListAsync(cancellation), totalCount);
        }
    }
}