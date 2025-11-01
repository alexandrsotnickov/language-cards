using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageCards.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "owner_name",
                table: "themes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "owner_name",
                table: "themes");
        }
    }
}
