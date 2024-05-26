
namespace TournamentAPI.Core.Repositories
{
    public interface ITournamentRepository
    {
        Task<IEnumerable<Tournament>> GetAllAsync(bool includeGames);

        Task<Tournament?> GetAsync(int id);

        Task<bool> AnyAsync(int id);

        void Add(Tournament tournament);

        void Update(Tournament tournament);

        void Remove(Tournament tournament);
    }
}
