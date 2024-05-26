
namespace TournamentAPI.Core.Repositories
{
    public interface IUnitOfWork
    {
        ITournamentRepository TournamentRepo { get; }

        IGameRepository GameRepo { get; }

        Task CompleteAsync();
    }
}
