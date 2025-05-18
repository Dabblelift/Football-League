using Football_League.Data.Models;

namespace Football_League.Repositories.Interfaces
{
    public interface ITeamRepository : IRepository<Team, int>
    {
        Task<bool> CheckIfTeamExistsAsync(string name);
    }
}
