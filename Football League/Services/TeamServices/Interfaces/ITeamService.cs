using Football_League.Data.DTOs;

namespace Football_League.Services.TeamServices.Interfaces
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamDTO>> GetAllTeamsAsync(ISortingStrategy sortingStrategy);

        Task AddTeamAsync(AddTeamDTO team);

        Task UpdateTeamAsync(UpdateTeamDTO team);

        Task DeleteTeamAsync(int id);
    }
}
