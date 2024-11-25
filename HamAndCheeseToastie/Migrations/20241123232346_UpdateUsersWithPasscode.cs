using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamAndCheeseToastie.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsersWithPasscode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Passcode",
                table: "users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Passcode",
                table: "users");
        }
    }
}
