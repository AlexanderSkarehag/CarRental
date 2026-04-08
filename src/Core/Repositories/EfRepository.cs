using System;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public class EfRepository<T, TContext> : EfRepository<T, Guid, TContext>, IRepository<T>
        where T : BaseEntity<Guid>
        where TContext : DbContext
    {
        public EfRepository(TContext dbContext)
            : base(dbContext)
        {
        }
    }

    public class EfRepository<T, Key, TContext> : IRepository<T, Key>
        where T : BaseEntity<Key>
        where TContext : DbContext
    {
        private protected TContext _dbContext;

        public EfRepository(TContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            entity.InsertedAt = DateTimeOffset.UtcNow;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _dbContext.Set<T>().AddAsync(entity);

            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(Key id)
        {
            var entity = await GetByIdAsync(id);

            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> GetByIdAsync(Key id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);

            return entity ?? throw new ArgumentNullException();
        }

        public async Task<IList<T>> ListAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<IList<T>> ListAllAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            entity.UpdatedAt = DateTimeOffset.UtcNow;

            _dbContext.Entry(entity).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
                throw;
            }
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return EfSpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }
    }
}
