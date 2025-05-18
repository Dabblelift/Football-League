using Football_League.Common;
using Football_League.Data.DTOs;
using Football_League.Data.Models;
using Football_League.Repositories.Interfaces;
using Football_League.Services.TeamServices.Interfaces;

namespace Football_League.Services.TeamServices
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork unitOfWork;
        public TeamService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void AddTeam(AddTeamDTO team)
        {
            var entryExists = unitOfWork.Teams.CheckIfTeamExists(team.TeamName);
            if (entryExists) throw new ArgumentException(string.Format(ErrorMessages.TeamAlreadyExists, team.TeamName));

            unitOfWork.Teams.Add(new Team()
            {
                Name = team.TeamName,
                Points = team.Points,
                GoalsScored = team.GoalsScored,
                GoalsCondeded = team.GoalsCondeded,
                Wins = team.Wins,
                Draws = team.Draws,
                Losses = team.Losses,
            });
            unitOfWork.Complete();
        }

        public void DeleteTeam(int id)
        {
            var entry = unitOfWork.Teams.GetById(id);

            unitOfWork.Teams.Delete(entry);
            unitOfWork.Complete();
        }

        public IEnumerable<TeamDTO> GetAllTeams(ISortingStrategy sortingStrategy)
        {
            return unitOfWork.Teams.GetAllAsNoTracking().Select(x => new TeamDTO()
            {
                Id = x.Id,
                TeamName = x.Name,
                Points = x.Points,
                GoalsScored = x.GoalsScored,
                GoalsCondeded = x.GoalsCondeded,
                Wins = x.Wins,
                Draws = x.Draws,
                Losses = x.Losses,
            });
        }

        public void Update(UpdateTeamDTO team)
        {
            var entry = unitOfWork.Teams.GetById(team.Id);

            var nameExists = unitOfWork.Teams.CheckIfTeamExists(team.TeamName);
            if (nameExists) throw new ArgumentException(string.Format(ErrorMessages.TeamAlreadyExists, team.TeamName));

            entry.Name = team.TeamName;
            entry.Points = team.Points;
            entry.GoalsScored = team.GoalsScored;
            entry.GoalsCondeded = team.GoalsCondeded;
            entry.Wins = team.Wins;
            entry.Draws = team.Draws;
            entry.Losses = team.Losses;

            unitOfWork.Complete();
        }
    }
}
