using Football_League.Data.Contexts;
using Football_League.Repositories.Interfaces;

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
        public int Complete() => context.SaveChanges();
        public void Dispose() => context.Dispose();

    }
}
