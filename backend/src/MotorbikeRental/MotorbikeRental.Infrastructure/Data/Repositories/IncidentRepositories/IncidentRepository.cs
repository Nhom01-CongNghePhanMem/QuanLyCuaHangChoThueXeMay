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
    }
}