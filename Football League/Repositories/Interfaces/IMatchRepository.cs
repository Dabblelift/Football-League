using Football_League.Data.Models;

namespace Football_League.Repositories.Interfaces
{
    public interface IMatchRepository : IRepository<Match, int>
    {
        IEnumerable<Match> GetMatchesWithTeams();
        Match GetMatchByIdWithTeams(int id);
    }
}
