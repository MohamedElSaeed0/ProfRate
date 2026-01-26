using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfRate.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Students_Username",
                table: "Students",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_Username",
                table: "Lecturers",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_Username",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Lecturers_Username",
                table: "Lecturers");
        }
    }
}
