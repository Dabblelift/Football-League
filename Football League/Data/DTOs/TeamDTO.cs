namespace Football_League.Data.DTOs
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int Points { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsCondeded { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
    }
}
