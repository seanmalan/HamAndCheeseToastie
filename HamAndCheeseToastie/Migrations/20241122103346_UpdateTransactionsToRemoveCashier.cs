using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HamAndCheeseToastie.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionsToRemoveCashier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transaction_cashier_cashierid",
                table: "transaction");

            migrationBuilder.DropTable(
                name: "cashier");

            migrationBuilder.DropIndex(
                name: "IX_transaction_cashierid",
                table: "transaction");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cashier",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cashier_id = table.Column<int>(type: "integer", nullable: false),
                    employee_code = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cashier", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transaction_cashierid",
                table: "transaction",
                column: "cashierid");

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_cashier_cashierid",
                table: "transaction",
                column: "cashierid",
                principalTable: "cashier",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
