namespace Core.Interfaces
{
    public interface IRepository<T, Key> where T : IAggregateRoot<Key>
    {
        Task<T> GetByIdAsync(Key id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> DeleteAsync(Key id);
        Task<IList<T>> ListAll();
        Task<IList<T>> ListAllAsync(ISpecification<T> spec);
    }
    public interface IRepository<T> : IRepository<T, Guid>
        where T : IAggregateRoot<Guid>
    {
    }
}
