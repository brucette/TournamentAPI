﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Data.Data;
using TournamentAPI.Data.Repositories;

namespace TournamentAPI.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TournamentAPIContext _context;

        public ITournamentRepository TournamentRepo { get; }

        public IGameRepository GameRepo { get; }

        public UnitOfWork(TournamentAPIContext context)
        {
            _context = context;
            TournamentRepo = new TournamentRepository(_context);    
            GameRepo= new GameRepository(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
