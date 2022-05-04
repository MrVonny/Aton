namespace Aton.Domain.Intefaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(Guid id);
        void Update(TEntity obj);
        void Remove(Guid id);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
