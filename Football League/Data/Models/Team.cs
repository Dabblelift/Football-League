using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Football_League.Shared.Entities;

namespace Football_League.Data.Models
{
    public class Team : BaseEntity<int>
    {
        [Required]
        public string Name { get; set; }
        public int Points { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsCondeded { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        [JsonIgnore]
        public virtual ICollection<Match> HomeMatches { get; set; } = new HashSet<Match>();
        [JsonIgnore]
        public virtual ICollection<Match> AwayMatches { get; set; } = new HashSet<Match>();
    }
}
