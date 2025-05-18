using Football_League.Data.Contexts;
using Football_League.Data.Models.Interfaces;
using Football_League.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace Football_League.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FootballLeagueDBContext context;

        public UnitOfWork(FootballLeagueDBContext db)
        {
            this.context = db;
            this.Teams = new TeamRepository(context);
            this.Matches = new MatchRepository(context);
        }
        public ITeamRepository Teams { get; private set; }
        public IMatchRepository Matches { get; private set; }
        public async Task<int> CompleteAsync() 
        {
            ApplyAuditInfoRules();
            return await context.SaveChangesAsync(); 
        }
        public void Dispose() => context.Dispose();
        private void ApplyAuditInfoRules()
        {
            var changedEntries = context.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IEntityAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IEntityAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }
    }
}
