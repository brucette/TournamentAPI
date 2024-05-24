using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStartDateToTimeInGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Games",
                newName: "Time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Games",
                newName: "StartDate");
        }
    }
}
