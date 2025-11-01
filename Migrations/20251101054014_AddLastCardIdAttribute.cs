using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageCards.Migrations
{
    /// <inheritdoc />
    public partial class AddLastCardIdAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "last_card_id",
                table: "themes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "last_card_id",
                table: "themes");
        }
    }
}
