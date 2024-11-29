using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HamAndCheeseToastie.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transaction_customer_customerid",
                table: "transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_transaction_item_products_productid",
                table: "transaction_item");

            migrationBuilder.DropForeignKey(
                name: "FK_transaction_item_transaction_transactionid",
                table: "transaction_item");

            migrationBuilder.DropIndex(
                name: "IX_transaction_item_productid",
                table: "transaction_item");

            migrationBuilder.CreateTable(
                name: "inventory_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    barcode = table.Column<string>(type: "text", nullable: false),
                    change_type = table.Column<string>(type: "text", nullable: true),
                    stock_old_value = table.Column<string>(type: "text", nullable: true),
                    stock_new_value = table.Column<string>(type: "text", nullable: true),
                    reduction_reason = table.Column<string>(type: "text", nullable: true),
                    name_old_value = table.Column<string>(type: "text", nullable: true),
                    name_new_value = table.Column<string>(type: "text", nullable: true),
                    brand_old_value = table.Column<string>(type: "text", nullable: true),
                    brand_new_value = table.Column<string>(type: "text", nullable: true),
                    category_old_value = table.Column<string>(type: "text", nullable: true),
                    category_new_value = table.Column<string>(type: "text", nullable: true),
                    price_old_value = table.Column<decimal>(type: "numeric", nullable: false),
                    price_new_value = table.Column<decimal>(type: "numeric", nullable: false),
                    wholesale_price_old_value = table.Column<decimal>(type: "numeric", nullable: false),
                    wholesale_price_new_value = table.Column<decimal>(type: "numeric", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_logs_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inventory_logs_product_id",
                table: "inventory_logs",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_customer_customerid",
                table: "transaction",
                column: "customerid",
                principalTable: "customer",
                principalColumn: "customerid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transaction_customer_customerid",
                table: "transaction");

            migrationBuilder.DropTable(
                name: "inventory_logs");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_item_productid",
                table: "transaction_item",
                column: "productid");

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_customer_customerid",
                table: "transaction",
                column: "customerid",
                principalTable: "customer",
                principalColumn: "customerid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_item_products_productid",
                table: "transaction_item",
                column: "productid",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_item_transaction_transactionid",
                table: "transaction_item",
                column: "transactionid",
                principalTable: "transaction",
                principalColumn: "transactionid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
