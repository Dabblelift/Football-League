using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Football_League.Data.DTOs
{
    public class UpdateTeamDTO
    {
        public int Id { get; set; }

        [MinLength(3)]
        public string TeamName { get; set; }

        [Range(0, 200)]
        [DefaultValue(0)]
        public int Points { get; set; }

        [Range(0, 200)]
        [DefaultValue(0)]
        public int GoalsScored { get; set; }

        [Range(0, 200)]
        [DefaultValue(0)]
        public int GoalsCondeded { get; set; }

        [Range(0, 100)]
        [DefaultValue(0)]
        public int Wins { get; set; }

        [Range(0, 100)]
        [DefaultValue(0)]
        public int Draws { get; set; }

        [Range(0, 100)]
        [DefaultValue(0)]
        public int Losses { get; set; }
    }
}
