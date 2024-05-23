using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly TournamentAPIContext _context;

        public TournamentRepository(TournamentAPIContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Tournament>> GetAllAsync()
        { 
            return await _context.Tournaments
                .Include(t => t.Games)
                .ToListAsync();    
        }

        public async Task<Tournament?> GetAsync(int id)
        { 
            return await _context.Tournaments
                .Include(t => t.Games)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Tournaments.AnyAsync(t => t.Id == id);
        }

        public void Add(Tournament tournament)
        {
             _context.Tournaments.Add(tournament);
        }

        public void Update(Tournament tournament)
        { 
            _context.Tournaments.Update(tournament);
        }

        public void Remove(Tournament tournament)
        { 
            _context.Tournaments.Remove(tournament);
        }
    }
}
