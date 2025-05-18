using System.Linq.Expressions;
using Football_League.Common;
using Football_League.Repositories.Interfaces;
using Football_League.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Football_League.Shared.Repositories
{
    public class EntityFrameworkRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public EntityFrameworkRepository(DbContext db)
        {
            Context = db;
            DbSet = Context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll() => DbSet.ToList();
        public virtual IEnumerable<TEntity> GetAllAsNoTracking() => DbSet.AsNoTracking().ToList();
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => DbSet.Where(predicate).ToList();
        public TEntity GetById(TKey id)
        {
            var entry = DbSet.FirstOrDefault(x => x.Id!.Equals(id));
            if (entry == null) throw new ArgumentException(string.Format(ErrorMessages.EntityNotFound, typeof(TEntity).Name, id));
            return entry;
        }
        public virtual void Add(TEntity entity) => DbSet.Add(entity);

        public virtual void Delete(TEntity entity) => DbSet.Remove(entity);
    }
}
