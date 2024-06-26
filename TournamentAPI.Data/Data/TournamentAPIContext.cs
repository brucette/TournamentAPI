﻿namespace TournamentAPI.Data.Data
{
    public class TournamentAPIContext : DbContext
    {
        public TournamentAPIContext (DbContextOptions<TournamentAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Tournament> Tournaments { get; set; } = default!;
        public DbSet<Game> Games { get; set; } = default!;
    }
}
