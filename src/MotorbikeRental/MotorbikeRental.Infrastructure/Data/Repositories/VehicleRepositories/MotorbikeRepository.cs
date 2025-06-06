using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.VehicleRepositories
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
        public async Task<(IEnumerable<Motorbike>, int totalCount)> GetFilterData(MotorbikeFilterDto motorbikeFilterDto)
        {
            IQueryable<Motorbike> queryable = dbContext.Motorbikes
                .Include(m => m.Category)
                .Include(m => m.PriceList)
                .AsQueryable();
            if (motorbikeFilterDto.CategoryId.HasValue)
                queryable = queryable.Where(m => m.CategoryId == motorbikeFilterDto.CategoryId);
            if (motorbikeFilterDto.Status.HasValue)
                queryable = queryable.Where(m => m.Status == motorbikeFilterDto.Status);
            if (!string.IsNullOrWhiteSpace(motorbikeFilterDto.Brand))
                queryable = queryable.Where(m => m.Brand == motorbikeFilterDto.Brand);
            int totalCount = await queryable.CountAsync();
            queryable = queryable.AsNoTracking().Skip((motorbikeFilterDto.PageNumber - 1) * motorbikeFilterDto.PageSize).Take(motorbikeFilterDto.PageSize);
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