using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Interfaces.IRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly MotorbikeRentalDbContext dbContext;
        public BaseRepository(MotorbikeRentalDbContext motorbikeRentalDbContext)
        {
            dbContext = motorbikeRentalDbContext;
        }

        public async Task<T> Create(T model)
        {
            dbContext.Set<T>().Add(model);
            await dbContext.SaveChangesAsync();
            return model;
        }

        public async Task Delete(T model)
        {
            dbContext.Set<T>().Remove(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetById<Tid>(Tid id)
        {
            T? model = await dbContext.Set<T>().FindAsync(id);
            return model != null ? model : throw new Exception("No data");
        }
        public async Task SaveChangeAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(T model)
        {
            dbContext.Set<T>().Update(model);
            await dbContext.SaveChangesAsync();
        }
    }
}