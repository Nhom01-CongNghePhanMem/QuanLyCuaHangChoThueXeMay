using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.VehicleRepositories
{
    public class MaintenanceRecordRepository : BaseRepository<MaintenanceRecord>, IMaintenanceRecordRepository
    {
        public MaintenanceRecordRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
    }
}