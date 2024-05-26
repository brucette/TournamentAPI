using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Core.Dto
{
    public class TournamentWithGamesDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate => StartDate.AddMonths(1);

        public ICollection<GameDto> Games { get; set; } = new List<GameDto>();
    }
}
