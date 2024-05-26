namespace TournamentAPI.Core.Dto
{
    public class TournamentForUpdateDto
    {
        [Required(ErrorMessage = "You should provide a title.")]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "You should provide a start date.")]
        public DateTime? StartDate { get; set; }
    }
}
