using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimitiveTextGame.Telegram.Modules.Games.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserEntityChangesv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPlayingGame",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSearchingForGame",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPlayingGame",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsSearchingForGame",
                table: "Users");
        }
    }
}
