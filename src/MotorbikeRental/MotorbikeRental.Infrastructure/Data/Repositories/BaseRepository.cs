using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Vehicles;
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
        public async Task<bool> IsExists<Tvalue>(string key, Tvalue value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);
            return await dbContext.Set<T>().AsNoTracking().AnyAsync(lambda);
        }
        public async Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value, string idPropertyName = "Id")
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);

            var idProperty = Expression.Property(parameter, idPropertyName);
            var idEquality = Expression.NotEqual(idProperty, Expression.Constant(id));

            var combinedExpression = Expression.AndAlso(equality, idEquality);
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            return await dbContext.Set<T>().AnyAsync(lambda);
        }
    }
}