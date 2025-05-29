using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.Incidents;
using MotorbikeRental.Core.Interfaces.IRepositories.IIncidents;
using MotorbikeRental.Infrastructure.Data;

namespace MotorbikeRental.Infrastructure.Repositories.IncidentRepositories
{
    public class IncidentImageRepository : BaseRepository<IncidentImage>, IIncidentImageRepository
    {
        public IncidentImageRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
    }
}