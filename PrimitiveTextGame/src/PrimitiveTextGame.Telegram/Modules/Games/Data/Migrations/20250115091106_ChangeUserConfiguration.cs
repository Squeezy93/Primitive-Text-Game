using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimitiveTextGame.Telegram.Modules.Games.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_CharacterId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CharacterId",
                table: "Users",
                column: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_CharacterId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CharacterId",
                table: "Users",
                column: "CharacterId",
                unique: true);
        }
    }
}
