using Football_League.Data.DTOs;

namespace Football_League.Services.MatchServices.Interfaces
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchDTO>> GetAllMatchesAsync();

        Task AddMatchAsync(AddMatchDTO match);
        Task UpdateMatchAsync(UpdateMatchDTO match);
        Task DeleteMatchAsync(int id);

    }
}
