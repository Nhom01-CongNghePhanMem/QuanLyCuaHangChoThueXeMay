using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.Vehicles;
using MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data;

namespace MotorbikeRental.Infrastructure.Repositories.VehicleRepositories
{
    public class MaintenanceRecordRepository : BaseRepository<MaintenanceRecord>, IMaintenanceRecordRepository
    {
        public MaintenanceRecordRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
    }
}