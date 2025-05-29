using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data;

namespace MotorbikeRental.Infrastructure.Repositories.VehicleRepositories
{
    public class MotorbikeRepository : BaseRepository<Motorbike>, IMotorbikeRepository
    {
        public MotorbikeRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task<bool> IsExists(int id)
        {
            return await dbContext.Set<Motorbike>().AsNoTracking().AnyAsync(m => m.MotorbikeId == id);
        }
        public async Task<bool> LicensePlateExists(string LicensePlate)
        {
            return await dbContext.Set<Motorbike>().AsNoTracking().AnyAsync(m => m.LicensePlate == m.LicensePlate);
        }
        public async Task<bool> ChassisNumberExists(string ChassisNumber)
        {
            return await dbContext.Set<Motorbike>().AsNoTracking().AnyAsync(m => m.ChassisNumber == ChassisNumber);
        }
        public async Task<bool> EngineNumberExists(string EngineNumber)
        {
            return await dbContext.Set<Motorbike>().AsNoTracking().AnyAsync(m => m.EngineNumber == EngineNumber);
        }
    }
}