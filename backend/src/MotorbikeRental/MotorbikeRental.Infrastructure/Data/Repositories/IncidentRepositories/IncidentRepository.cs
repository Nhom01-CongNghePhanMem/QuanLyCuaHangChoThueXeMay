using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Interfaces.IRepositories.IIncidents;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.IncidentRepositories
{
    public class IncidentRepository : BaseRepository<Incident>, IIncidentRepository
    {
        public IncidentRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<Incident?> GetIncidentByIdWithIncludes(int incidentId, bool isTracking, CancellationToken cancellationToken = default)
        {
            IQueryable<Incident> queryable = dbContext.Incidents.Where(i => i.IncidentId == incidentId)
                .Include(i => i.Images);
            if (!isTracking)
                queryable = queryable.AsNoTracking();
            return await queryable.FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<bool> IsExistsContractIdForUpdate(int incidentId, int contractId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Incidents.AnyAsync(i => i.ContractId == contractId && i.IncidentId != incidentId, cancellationToken);
        }
        public async Task<(IEnumerable<Incident>, int totalCount)> GetIncidentsByFilter(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, bool? isResolved, CancellationToken cancellationToken = default)
        {
            IQueryable<Incident> queryable = dbContext.Incidents.AsNoTracking()
                .OrderByDescending(i => i.IncidentId)
                .Include(e => e.Images);
            if (fromDate.HasValue)
                queryable = queryable.Where(i => i.IncidentDate > fromDate.Value);
            if (toDate.HasValue)
                queryable = queryable.Where(i => i.IncidentDate < toDate.Value);
            if (isResolved.HasValue)
                queryable = queryable.Where(i => isResolved.Value);
            int totalCount = await queryable.CountAsync(cancellationToken);
            queryable = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return (await queryable.ToListAsync(cancellationToken), totalCount);
        }
    }
}