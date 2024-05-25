using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Core.Repositories
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllAsync(int tournamentId);

        Task<Game?> GetAsync(int tournamentId, int gameId);

        Task<bool> AnyAsync(int tournamentId, int gameId);

        void Add(Game game);

        void Update(Game game);

        void Remove(Game game);

        Task<IEnumerable<Game>> GetAllGamesAsync();
    }
}
