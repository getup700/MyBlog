using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddSuggestionPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_Suggestion_Id",
                table: "Suggestion",
                column: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Suggestion_Id",
                table: "Suggestion");
        }
    }
}
