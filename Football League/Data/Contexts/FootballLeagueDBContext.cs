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
    }
}
