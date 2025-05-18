using Football_League.Data.Contexts;
using Football_League.Data.Models;
using Football_League.Repositories.Interfaces;
using Football_League.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Football_League.Repositories
{
    public class TeamRepository : EntityFrameworkRepository<Team, int>, ITeamRepository
    {
        private readonly FootballLeagueDBContext db;

        public TeamRepository(FootballLeagueDBContext db) : base(db)
        {
            this.db = db;
        }

        public async Task<bool> CheckIfTeamExistsAsync(string name)
        {
            return await db.Teams.AnyAsync(x => x.Name == name);
        }
    }
}
