using System.Linq.Expressions;
using Football_League.Shared.Entities;

namespace Football_League.Repositories.Interfaces
{
    public interface IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllAsNoTracking();
        TEntity GetById(TKey id);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void Delete(TEntity entity);
    }
}
