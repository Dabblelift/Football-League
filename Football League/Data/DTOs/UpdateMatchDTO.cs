using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Football_League.Data.DTOs
{
    public class UpdateMatchDTO
    {
        public int Id { get; set; }
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
