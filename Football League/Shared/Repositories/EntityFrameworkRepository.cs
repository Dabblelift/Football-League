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

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await DbSet.ToListAsync();
        public virtual async Task<IEnumerable<TEntity>> GetAllAsNoTrackingAsync() => await DbSet.AsNoTracking().ToListAsync();
        public async Task <IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) => await DbSet.Where(predicate).ToListAsync();
        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            var entry = await DbSet.FirstOrDefaultAsync(x => x.Id!.Equals(id));
            if (entry == null) throw new ArgumentException(string.Format(ErrorMessages.EntityNotFound, typeof(TEntity).Name, id));
            return entry;
        }
        public virtual async Task AddAsync(TEntity entity) => await DbSet.AddAsync(entity);

        public virtual void Delete(TEntity entity) => DbSet.Remove(entity);
    }
}
