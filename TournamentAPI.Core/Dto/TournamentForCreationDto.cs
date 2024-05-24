﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Dto
{
    public class TournamentForCreationDto
    {
        [Required(ErrorMessage = "You should provide a title.")]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "You should provide a start date.")]
        public DateTime StartDate { get; set; }
    }
}
