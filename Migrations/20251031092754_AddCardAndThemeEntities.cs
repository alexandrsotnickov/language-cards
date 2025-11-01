using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageCards.Migrations
{
    /// <inheritdoc />
    public partial class AddCardAndThemeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_cards_statuses",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    card_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_cards_statuses", x => new { x.user_id, x.card_id });
                    table.ForeignKey(
                        name: "fk_user_cards_statuses_cards_card_id",
                        column: x => x.card_id,
                        principalTable: "cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_cards_statuses_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserThemes",
                columns: table => new
                {
                    added_themes_id = table.Column<int>(type: "integer", nullable: false),
                    theme_subscribers_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_themes", x => new { x.added_themes_id, x.theme_subscribers_id });
                    table.ForeignKey(
                        name: "fk_user_themes_themes_added_themes_id",
                        column: x => x.added_themes_id,
                        principalTable: "themes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_themes_users_theme_subscribers_id",
                        column: x => x.theme_subscribers_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_cards_statuses_card_id",
                table: "user_cards_statuses",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_themes_theme_subscribers_id",
                table: "UserThemes",
                column: "theme_subscribers_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_cards_statuses");

            migrationBuilder.DropTable(
                name: "UserThemes");
        }
    }
}
