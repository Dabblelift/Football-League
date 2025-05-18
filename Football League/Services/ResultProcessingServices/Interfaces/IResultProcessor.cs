using Football_League.Data.Models;

namespace Football_League.Services.ResultProcessingServices.Interfaces
{
    public interface IResultProcessor
    {
        void Apply(Match match);
        void Revert(Match match);
    }
}
