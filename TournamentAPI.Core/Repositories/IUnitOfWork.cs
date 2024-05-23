﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Repositories
{
    public interface IUnitOfWork
    {
        ITournamentRepository TournamentRepo { get; }

        IGameRepository GameRepo { get; }

        Task CompleteAsync();
    }
}