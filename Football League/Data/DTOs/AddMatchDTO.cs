using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Football_League.Data.DTOs
{
    public class AddMatchDTO
    {
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }

        [Range(0, 200)]
        [DefaultValue(0)]
        public int HomeTeamGoals { get; set; }

        [Range(0, 200)]
        [DefaultValue(0)]
        public int AwayTeamGoals { get; set; }
    }
}
