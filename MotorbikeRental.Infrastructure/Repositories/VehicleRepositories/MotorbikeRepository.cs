using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Core.Entities.Business.Pagination;
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
        public async Task<Motorbike> GetByIdNoAsTracking(int id)
        {
            return await dbContext.Motorbikes.Where(c => c.MotorbikeId == id).AsNoTracking().FirstAsync() ?? throw new Exception("Category not found");
        }
        public async Task<bool> DupEngineNumExceptId(string engineNumber, int id)
        {
            return await dbContext.Motorbikes.AsNoTracking().AnyAsync(m => m.EngineNumber == engineNumber && m.MotorbikeId != id);
        }
        public async Task<bool> DupChassisNumExceptId(string chassisNumber, int id)
        {
            return await dbContext.Motorbikes.AsNoTracking().AnyAsync(m => m.ChassisNumber == chassisNumber && m.MotorbikeId != id);
        }
        public async Task<bool> DupLicensePlateExceptId(string licensePlate, int id)
        {
            return await dbContext.Motorbikes.AsNoTracking().AnyAsync(m => m.LicensePlate == licensePlate && m.MotorbikeId != id);
        }
        public async Task<PaginatedDataViewModel<Motorbike>> GetMotorbikesByCategory(int pageNumber, int pageSize, int categoryId)
        {
            IQueryable<Motorbike> query = dbContext.Motorbikes.Where(s => s.CategoryId == categoryId);
            return new PaginatedDataViewModel<Motorbike>(await query.AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(), await query.AsNoTracking().CountAsync());
        }
    }
}