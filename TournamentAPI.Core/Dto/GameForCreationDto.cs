using System.ComponentModel.DataAnnotations;

namespace TournamentAPI.Core.Dto
{
    public class GameForCreationDto
    {
        [Required(ErrorMessage = "You should provide a title")]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "You should provide a start time.")]
        public DateTime? Time { get; set; }
    }
}
