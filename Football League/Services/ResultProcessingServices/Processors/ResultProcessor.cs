using Football_League.Data.Models;
using Football_League.Services.ResultProcessingServices.Interfaces;

namespace Football_League.Services.ResultProcessingServices.Processors
{
    public class ResultProcessor : IResultProcessor
    {
        private readonly IScoringStrategy scoringStrategy;

        public ResultProcessor(IScoringStrategy scoringStrategy) 
        {
            this.scoringStrategy = scoringStrategy;
        }
        public void Apply(Match match)
        {
            scoringStrategy.ApplyScore(match);
        }

        public void Revert(Match match)
        {
            scoringStrategy.RevertScore(match);
        }
    }
}
