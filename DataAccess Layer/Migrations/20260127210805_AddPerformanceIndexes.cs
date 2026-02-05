using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfRate.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Students_FirstName",
                table: "Students",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Students_LastName",
                table: "Students",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_FirstName",
                table: "Lecturers",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_LastName",
                table: "Lecturers",
                column: "LastName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_FirstName",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_LastName",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Lecturers_FirstName",
                table: "Lecturers");

            migrationBuilder.DropIndex(
                name: "IX_Lecturers_LastName",
                table: "Lecturers");
        }
    }
}
