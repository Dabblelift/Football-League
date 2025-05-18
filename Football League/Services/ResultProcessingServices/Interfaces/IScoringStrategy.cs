using Football_League.Data.Models;

namespace Football_League.Services.ResultProcessingServices.Interfaces
{
    public interface IScoringStrategy
    {
        void ApplyScore(Match match);
        void RevertScore(Match match);
    }
}
