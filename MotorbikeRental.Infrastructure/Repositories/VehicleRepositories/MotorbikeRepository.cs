using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Core.Entities.Business.Pagination;
using MotorbikeRental.Core.Entities.Business.Vehicles;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Enums;
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
            return await dbContext.Set<Motorbike>().AsNoTracking().AnyAsync(m => m.LicensePlate == LicensePlate);
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
            return await dbContext.Motorbikes.Where(c => c.MotorbikeId == id).AsNoTracking().FirstOrDefaultAsync() ?? throw new Exception("Category not found");
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
        public async Task<Motorbike> GetByIdWithIncludes(int id)
        {
            return await dbContext.Motorbikes.Where(m => m.MotorbikeId == id)
                .AsNoTracking()
                .Include(m => m.Category)
                .Include(m => m.PriceList)
                .Include(m => m.Incidents)
                .FirstOrDefaultAsync() ?? throw new Exception("Motorbike not found");
        }
        public async Task<(IEnumerable<Motorbike>, int totalCount)> GetFilterData(MotorbikeFilterViewModel motorbikeFilterViewModel)
        {
            IQueryable<Motorbike> queryable = dbContext.Motorbikes
                .Include(m => m.Category)
                .Include(m => m.PriceList)
                .AsQueryable();
            if (motorbikeFilterViewModel.CategoryId.HasValue)
                queryable = queryable.Where(m => m.CategoryId == motorbikeFilterViewModel.CategoryId);
            if (motorbikeFilterViewModel.Status.HasValue)
                queryable = queryable.Where(m => m.Status == motorbikeFilterViewModel.Status);
            if (!string.IsNullOrWhiteSpace(motorbikeFilterViewModel.Brand))
                queryable = queryable.Where(m => m.Brand == motorbikeFilterViewModel.Brand);
            int totalCount = await queryable.CountAsync();
            queryable = queryable.AsNoTracking().Skip((motorbikeFilterViewModel.PageNumber - 1) * motorbikeFilterViewModel.PageSize).Take(motorbikeFilterViewModel.PageSize);
            return (await queryable.ToListAsync(), totalCount);
        }
        public async Task<IEnumerable<string>> GetDistinctBrands()
        {
            return await dbContext.Motorbikes
                .Where(m => !string.IsNullOrEmpty(m.Brand))
                .Select(m => m.Brand)
                .Distinct()
                .ToListAsync();
        }
    }
}