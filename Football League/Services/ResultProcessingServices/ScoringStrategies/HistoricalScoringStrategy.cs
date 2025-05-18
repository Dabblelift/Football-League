using Football_League.Data.Models;
using Football_League.Services.ResultProcessingServices.Interfaces;

namespace Football_League.Services.ResultProcessingServices.ScoringStrategies
{
    public class HistoricalScoringStrategy : IScoringStrategy
    {
        public void ApplyScore(Match match) => UpdateStats(match, false);
        public void RevertScore(Match match) => UpdateStats(match, true);
        private void UpdateStats(Match match, bool isRevert)
        {
            var mod = isRevert ? -1 : 1;

            var homeTeam = match.HomeTeam;
            var awayTeam = match.AwayTeam;

            homeTeam.GoalsScored += mod * match.HomeTeamGoals;
            homeTeam.GoalsCondeded += mod * match.AwayTeamGoals;

            awayTeam.GoalsScored += mod * match.AwayTeamGoals;
            awayTeam.GoalsCondeded += mod * match.HomeTeamGoals;

            if (match.HomeTeamGoals > match.AwayTeamGoals)
            {
                homeTeam.Wins += mod;
                homeTeam.Points += 2 * mod;
                awayTeam.Losses += mod;
            }
            else if (match.HomeTeamGoals < match.AwayTeamGoals)
            {
                awayTeam.Wins += mod;
                awayTeam.Points += 2 * mod;
                homeTeam.Losses += mod;
            }
            else
            {
                homeTeam.Draws += mod;
                awayTeam.Draws += mod;
                homeTeam.Points += 1 * mod;
                awayTeam.Points += 1 * mod;
            }
        }
    }
}
