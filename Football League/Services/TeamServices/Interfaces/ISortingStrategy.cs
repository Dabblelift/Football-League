using Football_League.Data.DTOs;

namespace Football_League.Services.TeamServices.Interfaces
{
    public interface ISortingStrategy
    {
        IEnumerable<TeamDTO> Sort(IEnumerable<TeamDTO> teams);
    }
}
