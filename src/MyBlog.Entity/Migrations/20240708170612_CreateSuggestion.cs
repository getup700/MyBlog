using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class CreateSuggestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suggestion",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Suggestion");
        }
    }
}
