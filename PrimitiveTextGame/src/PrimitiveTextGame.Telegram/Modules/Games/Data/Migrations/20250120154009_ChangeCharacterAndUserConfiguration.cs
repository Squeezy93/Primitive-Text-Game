using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimitiveTextGame.Telegram.Modules.Games.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCharacterAndUserConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Health",
                table: "Characters");

            migrationBuilder.AddColumn<int>(
                name: "Health",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Health",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "Health",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
