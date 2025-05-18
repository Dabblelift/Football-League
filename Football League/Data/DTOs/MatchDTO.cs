namespace Football_League.Data.DTOs
{
    public class MatchDTO
    {
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public string HomeTeamName { get; set; } = default!;
        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; } = default!;
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
    }
}
