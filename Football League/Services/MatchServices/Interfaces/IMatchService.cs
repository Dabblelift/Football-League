using Football_League.Data.DTOs;

namespace Football_League.Services.MatchServices.Interfaces
{
    public interface IMatchService
    {
        IEnumerable<MatchDTO> GetAllMatches();

        void AddMatch(AddMatchDTO match);
        void UpdateMatch(UpdateMatchDTO match);
        void DeleteMatch(int id);

    }
}
