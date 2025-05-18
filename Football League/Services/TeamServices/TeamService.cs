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

        public async Task AddTeamAsync(AddTeamDTO team)
        {
            var entryExists = await unitOfWork.Teams.CheckIfTeamExistsAsync(team.TeamName);
            if (entryExists) throw new ArgumentException(string.Format(ErrorMessages.TeamAlreadyExists, team.TeamName));

            await unitOfWork.Teams.AddAsync(new Team()
            {
                Name = team.TeamName,
                Points = team.Points,
                GoalsScored = team.GoalsScored,
                GoalsCondeded = team.GoalsCondeded,
                Wins = team.Wins,
                Draws = team.Draws,
                Losses = team.Losses,
            });

            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteTeamAsync(int id)
        {
            var entry = await unitOfWork.Teams.GetByIdAsync(id);

            unitOfWork.Teams.Delete(entry);
            await unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<TeamDTO>> GetAllTeamsAsync(ISortingStrategy sortingStrategy)
        {
            var teams = await unitOfWork.Teams.GetAllAsNoTrackingAsync();

            return teams.Select(x => new TeamDTO()
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

        public async Task UpdateTeamAsync(UpdateTeamDTO team)
        {
            var entry = await unitOfWork.Teams.GetByIdAsync(team.Id);
            if (entry.Name != team.TeamName)
            {
                var nameExists = await unitOfWork.Teams.CheckIfTeamExistsAsync(team.TeamName);
                if (nameExists) throw new ArgumentException(string.Format(ErrorMessages.TeamAlreadyExists, team.TeamName));
            }
            
            entry.Name = team.TeamName;
            entry.Points = team.Points;
            entry.GoalsScored = team.GoalsScored;
            entry.GoalsCondeded = team.GoalsCondeded;
            entry.Wins = team.Wins;
            entry.Draws = team.Draws;
            entry.Losses = team.Losses;

            await unitOfWork.CompleteAsync();
        }
    }
}
