
namespace TournamentAPI.Data.Data
{
    public class SeedData
    {
        public static string[] tournamentNames = new string[]
        { "Global Championship Series", "Grand Masters Open",
            "Ultimate Cup Finals", "Prestige Battle Royale", "Champions Cup Showdown"
        };

        public static string[] gameNames = new string[]
        {
           "Champion's Clash", "Victory Round", "Elite Duel", "Grand Showdown",
            "Supreme Battle", "Master's Faceoff", "Ultimate Standoff", "Premier Bout",
            "Final Confrontation", "Pro-Am Encounter"
        };

        public static int numOfGamesPerTourn = 2;

        public static async Task Initialize(TournamentAPIContext context)
        {
            // check if any tournaments exist
            if (await context.Tournaments.AnyAsync()) return;

            // add tournaments to the context
            var tournaments = GenerateTournaments(tournamentNames);
            await context.Tournaments.AddRangeAsync(tournaments);
            await context.SaveChangesAsync();

            // get the tournaments with their ids
            var tournamentsFromDb = await context.Tournaments.ToListAsync();

            // add games to the context
            var games = GenerateGames(tournamentsFromDb, numOfGamesPerTourn);
            await context.Games.AddRangeAsync(games);
            await context.SaveChangesAsync();
        }

        private static IEnumerable<Tournament> GenerateTournaments(IEnumerable<string> tournamentNames)
        {
            var rnd = new Random();

            var tournaments = new List<Tournament>();

            foreach (var tournamentName in tournamentNames)
            {
                var newTournament = new Tournament
                {
                    Title = tournamentName,
                    StartDate = DateTime.Now.AddDays(rnd.Next(1, 60))
                };

                tournaments.Add(newTournament);
            }
            return tournaments;
        }

        private static IEnumerable<Game> GenerateGames(IEnumerable<Tournament> tournaments, int numberOfGames)
        {
            var rnd = new Random();

            var games = new List<Game>();
            int index = 0;

            foreach (Tournament item in tournaments)
            {
                for (int i = 0; i < numberOfGames; i++)
                {
                    var newGame = new Game
                    {
                        Title = gameNames[index],
                        Time = item.StartDate.AddDays(rnd.Next(1, 10)),
                        TournamentId = item.Id
                    };

                    games.Add(newGame);

                    // Wrap around using the modulo operator so index doesn't go out of bound
                    index = (index + 1) % gameNames.Length;
                }
            }
            return games;
        }
    }
}
