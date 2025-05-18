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

        public async Task<IEnumerable<Match>> GetMatchesWithTeamsAsync()
        {
            return await db.Matches.Include(x => x.HomeTeam).Include(x => x.AwayTeam).ToListAsync();
        }
        public async Task<Match> GetMatchByIdWithTeamsAsync(int id)
        {
            var entry = await db.Matches.Include(x => x.HomeTeam).Include(x => x.AwayTeam).FirstOrDefaultAsync(x => x.Id == id);
            if (entry == null) throw new ArgumentException(string.Format(ErrorMessages.EntityNotFound, nameof(Match), id));
            return entry;
        }
    }
}
