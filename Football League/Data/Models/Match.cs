using System.ComponentModel.DataAnnotations.Schema;
using Football_League.Shared.Entities;

namespace Football_League.Data.Models
{
    public class Match : BaseEntity<int>
    {
        [ForeignKey(nameof(HomeTeam))]
        public int HomeTeamId { get; set; }
        public virtual Team HomeTeam { get; set; }
        [ForeignKey(nameof(AwayTeam))]
        public int AwayTeamId { get; set; }
        public virtual Team AwayTeam { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
    }
}
