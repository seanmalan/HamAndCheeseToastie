using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamAndCheeseToastie.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionsToChangeCashierToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cashierid",
                table: "transaction",
                newName: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userid",
                table: "transaction",
                newName: "cashierid");
        }
    }
}
