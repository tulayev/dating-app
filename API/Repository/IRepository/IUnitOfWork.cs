namespace API.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class;

        Task AddAsync<TEntity>(TEntity row) where TEntity : class;
        
        Task AddRangeAsync<TEntity>(IEnumerable<TEntity> rows) where TEntity : class;

        void Update<TEntity>(TEntity entity) where TEntity : class;

        void Delete<TEntity>(TEntity entity) where TEntity : class;

        void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        void DeleteRange<TEntity>(Func<TEntity, bool> where) where TEntity : class;

        Task SaveChangesAsync();
    }
}