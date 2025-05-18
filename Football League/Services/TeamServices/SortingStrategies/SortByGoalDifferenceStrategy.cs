using Football_League.Data.DTOs;
using Football_League.Services.TeamServices.Interfaces;

namespace Football_League.Services.TeamServices.SortingStrategies
{
    public class SortByGoalDifferenceStrategy : ISortingStrategy
    {
        public IEnumerable<TeamDTO> Sort(IEnumerable<TeamDTO> teams)
        {
            return teams.OrderByDescending(t => t.Points).ThenByDescending(t => t.GoalsScored - t.GoalsCondeded).ThenByDescending(t => t.GoalsScored).ThenByDescending(t => t.Wins);
        }
    }
}
