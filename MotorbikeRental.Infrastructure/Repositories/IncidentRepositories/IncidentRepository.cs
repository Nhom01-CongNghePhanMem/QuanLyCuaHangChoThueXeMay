using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.Incidents;
using MotorbikeRental.Core.Interfaces.IRepositories.IIncidents;
using MotorbikeRental.Infrastructure.Data;

namespace MotorbikeRental.Infrastructure.Repositories.IncidentRepositories
{
    public class IncidentRepository : BaseRepository<Incident> , IIncidentRepository
    {
        public IncidentRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext){}
    }
}