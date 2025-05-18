using Football_League.Common;
using Football_League.Data.Contexts;
using Football_League.Data.Models;
using Football_League.Repositories.Interfaces;
using Football_League.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Football_League.Repositories
{
    public class MatchRepository : EntityFrameworkRepository<Match, int>, IMatchRepository
    {
        private readonly FootballLeagueDBContext db;

        public MatchRepository(FootballLeagueDBContext db) : base(db)
        {
            this.db = db;
        }

        public IEnumerable<Match> GetMatchesWithTeams()
        {
            return db.Matches.Include(x => x.HomeTeam).Include(x => x.AwayTeam).ToList();
        }
        public Match GetMatchByIdWithTeams(int id)
        {
            var entry = db.Matches.Include(x => x.HomeTeam).Include(x => x.AwayTeam).FirstOrDefault(x => x.Id == id);
            if (entry == null) throw new ArgumentException(string.Format(ErrorMessages.EntityNotFound, nameof(Match), id));
            return entry;
        }
    }
}
