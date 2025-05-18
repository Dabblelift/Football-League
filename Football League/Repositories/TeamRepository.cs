using Football_League.Data.Contexts;
using Football_League.Data.Models;
using Football_League.Repositories.Interfaces;
using Football_League.Shared.Repositories;

namespace Football_League.Repositories
{
    public class TeamRepository : EntityFrameworkRepository<Team, int>, ITeamRepository
    {
        private readonly FootballLeagueDBContext db;

        public TeamRepository(FootballLeagueDBContext db) : base(db)
        {
            this.db = db;
        }

        public bool CheckIfTeamExists(string name)
        {
            return db.Teams.Any(x => x.Name == name);
        }
    }
}
