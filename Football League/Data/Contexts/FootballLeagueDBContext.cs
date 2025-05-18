using Football_League.Data.Models;
using Football_League.Data.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Football_League.Data.Contexts
{
    public class FootballLeagueDBContext : DbContext
    {
        public FootballLeagueDBContext() 
        { 
        
        }
        public FootballLeagueDBContext(DbContextOptions<FootballLeagueDBContext> options)
            : base(options)
        {

        }

        public DbSet<Match> Matches { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(x =>
            {
                x.HasMany(m => m.HomeMatches)
               .WithOne(ht => ht.HomeTeam)
               .OnDelete(DeleteBehavior.NoAction);

                x.HasMany(m => m.AwayMatches)
                .WithOne(al => al.AwayTeam)
                .OnDelete(DeleteBehavior.NoAction);
            });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ApplyAuditInfoRules();
            return base.SaveChanges(true);
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = ChangeTracker
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
