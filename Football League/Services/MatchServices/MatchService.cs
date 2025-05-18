using Football_League.Common;
using Football_League.Data.DTOs;
using Football_League.Data.Models;
using Football_League.Repositories.Interfaces;
using Football_League.Services.MatchServices.Interfaces;
using Football_League.Services.ResultProcessingServices.Interfaces;

namespace Football_League.Services.MatchServices
{
    public class MatchService : IMatchService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IResultProcessor resultProcessor;

        public MatchService(IUnitOfWork unitOfWork, IResultProcessor resultProcessor)
        {
            this.unitOfWork = unitOfWork;
            this.resultProcessor = resultProcessor;
        }
        public async Task AddMatchAsync(AddMatchDTO match)
        {
            if (match.HomeTeamId == match.AwayTeamId) throw new ArgumentException(ErrorMessages.MatchSameTeams);

            var homeTeam = await unitOfWork.Teams.GetByIdAsync(match.HomeTeamId);
            var awayTeam = await unitOfWork.Teams.GetByIdAsync(match.AwayTeamId);

            var entry = new Match()
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeamGoals = match.AwayTeamGoals,
            };

            await unitOfWork.Matches.AddAsync(entry);
            resultProcessor.Apply(entry);

            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteMatchAsync(int id)
        {
            var entry = await unitOfWork.Matches.GetByIdAsync(id);

            unitOfWork.Matches.Delete(entry);
            await unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<MatchDTO>> GetAllMatchesAsync()
        {
            var matches = await unitOfWork.Matches.GetMatchesWithTeamsAsync();

            return matches.Select(x => new MatchDTO() 
            { 
                Id = x.Id,
                HomeTeamId = x.HomeTeamId,
                AwayTeamId = x.AwayTeamId,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                HomeTeamGoals = x.HomeTeamGoals,
                AwayTeamGoals = x.AwayTeamGoals,
            });
        }

        public async Task UpdateMatchAsync(UpdateMatchDTO match)
        {
            if (match.HomeTeamId == match.AwayTeamId) throw new ArgumentException(ErrorMessages.MatchSameTeams);

            var entry = await unitOfWork.Matches.GetMatchByIdWithTeamsAsync(match.Id);
            entry.HomeTeam = await unitOfWork.Teams.GetByIdAsync(match.HomeTeamId);
            entry.AwayTeam = await unitOfWork.Teams.GetByIdAsync(match.AwayTeamId);

            resultProcessor.Revert(entry);

            entry.HomeTeamId = match.HomeTeamId;
            entry.AwayTeamId = match.AwayTeamId;

            entry.HomeTeamGoals = match.HomeTeamGoals;
            entry.AwayTeamGoals = match.AwayTeamGoals;

            resultProcessor.Apply(entry);

            await unitOfWork.CompleteAsync();
        }
    }
}
