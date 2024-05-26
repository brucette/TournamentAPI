namespace TournamentAPI.Core.Dto
{
    public class TournamentDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate => StartDate.AddMonths(1);
    }
}
