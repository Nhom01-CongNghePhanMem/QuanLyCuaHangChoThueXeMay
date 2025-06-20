namespace MotorbikeRental.Domain.Interfaces.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
        Task<T> GetById<Tid>(Tid id, CancellationToken cancellationToken = default);
        Task<T> Create(T model, CancellationToken cancellationToken = default);
        Task Update(T model, CancellationToken cancellationToken = default);
        Task Delete(T model, CancellationToken cancellationToken = default);
        Task SaveChangeAsync(CancellationToken cancellationToken = default);
        Task<bool> IsExists<Tvalue>(string key, Tvalue value, CancellationToken cancellationToken = default);
        Task<bool> IsExistsForUpdate<Tid>(Tid id, string tableName, string key, string value, CancellationToken cancellationToken = default);
    }
}