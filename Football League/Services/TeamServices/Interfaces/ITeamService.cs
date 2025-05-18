using Football_League.Data.DTOs;

namespace Football_League.Services.TeamServices.Interfaces
{
    public interface ITeamService
    {
        IEnumerable<TeamDTO> GetAllTeams(ISortingStrategy sortingStrategy);

        void AddTeam(AddTeamDTO team);

        void Update(UpdateTeamDTO team);

        void DeleteTeam(int id);
    }
}
