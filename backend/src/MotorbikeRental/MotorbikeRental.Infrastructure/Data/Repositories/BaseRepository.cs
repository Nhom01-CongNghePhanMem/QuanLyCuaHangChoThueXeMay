using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Interfaces.IRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;
using MotorbikeRental.Application.Exceptions;

namespace MotorbikeRental.Infrastructure.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly MotorbikeRentalDbContext dbContext;
        public BaseRepository(MotorbikeRentalDbContext motorbikeRentalDbContext)
        {
            dbContext = motorbikeRentalDbContext;
        }

        public async Task<T> Create(T model, CancellationToken cancellationToken = default)
        {
            dbContext.Set<T>().Add(model);
            await dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task Delete(T model, CancellationToken cancellationToken = default)
        {
            dbContext.Set<T>().Remove(model);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default)
        {
            return await dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T> GetById<Tid>(Tid id, CancellationToken cancellationToken = default)
        {
            T? model = await dbContext.Set<T>().FindAsync(id, cancellationToken);
            return model != null ? model : throw new NotFoundException("No data");
        }
        public async Task SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(T model, CancellationToken cancellationToken = default)
        {
            dbContext.Set<T>().Update(model);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<bool> IsExists<Tvalue>(string key, Tvalue value, CancellationToken cancellationToken = default)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);
            return await dbContext.Set<T>().AsNoTracking().AnyAsync(lambda, cancellationToken);
        }
        public async Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value, string idPropertyName = "Id", CancellationToken cancellationToken = default)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);

            var idProperty = Expression.Property(parameter, idPropertyName);
            var idEquality = Expression.NotEqual(idProperty, Expression.Constant(id));

            var combinedExpression = Expression.AndAlso(equality, idEquality);
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            return await dbContext.Set<T>().AsNoTracking().AnyAsync(lambda, cancellationToken);
        }
    }
}