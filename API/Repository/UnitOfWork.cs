using API.Repository.IRepository;
using Data;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class =>
            _context.Set<TEntity>();

        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class =>
            await _context.Set<TEntity>().AddAsync(entity);

        public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class =>
            await _context.Set<TEntity>().AddRangeAsync(entities);
        
        public void Update<TEntity>(TEntity entity) where TEntity : class =>
            _context.Entry(entity).State = EntityState.Modified;

        public void Delete<TEntity>(TEntity entity) where TEntity : class =>
            _context.Set<TEntity>().Remove(entity);

        public void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class => 
            _context.Set<TEntity>().RemoveRange(entities);

        public void DeleteRange<TEntity>(Func<TEntity, bool> predicate) where TEntity : class => 
            _context.Set<TEntity>().RemoveRange(_context.Set<TEntity>().Where(predicate));

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}