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
        public void AddMatch(AddMatchDTO match)
        {
            if (match.HomeTeamId == match.AwayTeamId) throw new ArgumentException(ErrorMessages.MatchSameTeams);

            var homeTeam = unitOfWork.Teams.GetById(match.HomeTeamId);
            var awayTeam = unitOfWork.Teams.GetById(match.AwayTeamId);

            var entry = new Match()
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeamGoals = match.AwayTeamGoals,
            };

            unitOfWork.Matches.Add(entry);
            resultProcessor.Apply(entry);

            unitOfWork.Complete();
        }

        public void DeleteMatch(int id)
        {
            var entry = unitOfWork.Matches.GetById(id);

            unitOfWork.Matches.Delete(entry);
            unitOfWork.Complete();
        }

        public IEnumerable<MatchDTO> GetAllMatches()
        {
            return unitOfWork.Matches.GetMatchesWithTeams().Select(x => new MatchDTO() 
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

        public void UpdateMatch(UpdateMatchDTO match)
        {
            if (match.HomeTeamId == match.AwayTeamId) throw new ArgumentException(ErrorMessages.MatchSameTeams);

            var entry = unitOfWork.Matches.GetMatchByIdWithTeams(match.Id);
            entry.HomeTeam = unitOfWork.Teams.GetById(match.HomeTeamId);
            entry.AwayTeam = unitOfWork.Teams.GetById(match.AwayTeamId);

            resultProcessor.Revert(entry);

            entry.HomeTeamId = match.HomeTeamId;
            entry.AwayTeamId = match.AwayTeamId;

            entry.HomeTeamGoals = match.HomeTeamGoals;
            entry.AwayTeamGoals = match.AwayTeamGoals;

            resultProcessor.Apply(entry);

            unitOfWork.Complete();
        }
    }
}
