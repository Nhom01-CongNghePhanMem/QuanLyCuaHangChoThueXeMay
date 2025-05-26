using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Core.Entities.Business.Pagination;
using MotorbikeRental.Core.Entities.General.User;
using MotorbikeRental.Core.Interfaces.IRepositories;
using MotorbikeRental.Infrastructure.Data;

namespace MotorbikeRental.Infrastructure.Repositories
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

        public async Task<PaginatedDataViewModel<T>> GetPaginatedData(int pageNumber, int pageSize)
        {
            IQueryable<T> query = dbContext.Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();
            IEnumerable<T> data = await query.ToListAsync();
            int totalCount = dbContext.Set<T>().Count();
            return new PaginatedDataViewModel<T>(data, totalCount);
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