namespace TournamentAPI.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly TournamentAPIContext _context;

        public GameRepository(TournamentAPIContext context) 
        {
            _context = context; 
        }

        public async Task<IEnumerable<Game>> GetAllAsync(int tournamentId)
        {
            return await _context.Games
                .Where(g => g.TournamentId == tournamentId)
                .ToListAsync();
        }

        public async Task<Game?> GetAsync(int tournamentId, int gameId)
        {
            return await _context.Games
                .Where(g => g.TournamentId == tournamentId)
                .FirstOrDefaultAsync(g => g.Id == gameId);
        }

        public async Task<Game?> GetByTitleAsync(int tournamentId, string title)
        {
            return await _context.Games
                .Where(g => g.TournamentId == tournamentId)
                .FirstOrDefaultAsync(g => g.Title == title);
        }

        public async Task<bool> AnyAsync(int tournamentId, int gameId)
        {
            return await _context.Games.AnyAsync(g => g.TournamentId == tournamentId && g.Id == gameId);
        }

        public void Add(Game game)
        {
            _context.Games.Add(game);   
        }

        public void Update(Game game)
        {
            _context.Games.Update(game);
        }

        public void Remove(Game game)
        {
            _context.Games.Remove(game);
        }

        // Get all games of all tournaments
        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            return await _context.Games.ToListAsync();
        }
    }
}
