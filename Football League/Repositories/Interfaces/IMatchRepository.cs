using Football_League.Data.Models;

namespace Football_League.Repositories.Interfaces
{
    public interface IMatchRepository : IRepository<Match, int>
    {
        Task<IEnumerable<Match>> GetMatchesWithTeamsAsync();
        Task<Match> GetMatchByIdWithTeamsAsync(int id);
    }
}
