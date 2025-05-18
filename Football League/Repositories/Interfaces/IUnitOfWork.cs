namespace Football_League.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITeamRepository Teams { get; }
        IMatchRepository Matches { get; }
        Task<int> CompleteAsync();
    }
}
