using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HamAndCheeseToastie.Migrations
{
    /// <inheritdoc />
    public partial class AddBarcodeToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Conditionally create tables if they do not exist
            if (!TableExists(migrationBuilder, "Customer"))
            {
                migrationBuilder.CreateTable(
                    name: "Customer",
                    columns: table => new
                    {
                        CustomerId = table.Column<int>(type: "integer", nullable: false)
                            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                        FirstName = table.Column<string>(type: "text", nullable: false),
                        LastName = table.Column<string>(type: "text", nullable: false),
                        Email = table.Column<string>(type: "text", nullable: true),
                        PhoneNumber = table.Column<string>(type: "text", nullable: true),
                        IsLoyaltyMember = table.Column<bool>(type: "boolean", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Customer", x => x.CustomerId);
                    });
            }

            // Add Barcode column to Customer if it does not already exist
            if (!ColumnExists(migrationBuilder, "Customer", "Barcode"))
            {
                migrationBuilder.AddColumn<string>(
                    name: "Barcode",
                    table: "Customer",
                    type: "text",
                    nullable: false,
                    defaultValue: "");
            }

            // Repeat for other tables and relationships
            // Example for another table (Roles), assuming same pattern
            if (!TableExists(migrationBuilder, "Roles"))
            {
                migrationBuilder.CreateTable(
                    name: "Roles",
                    columns: table => new
                    {
                        id = table.Column<int>(type: "integer", nullable: false)
                            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                        Name = table.Column<string>(type: "text", nullable: false),
                        UserId = table.Column<int>(type: "integer", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Roles", x => x.id);
                    });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop Barcode column
            if (ColumnExists(migrationBuilder, "Customer", "Barcode"))
            {
                migrationBuilder.DropColumn(
                    name: "Barcode",
                    table: "Customer");
            }

            // Drop tables if needed
            migrationBuilder.DropTable(name: "Roles");
            migrationBuilder.DropTable(name: "Customer");
        }

        private bool TableExists(MigrationBuilder migrationBuilder, string tableName)
        {
            var command = $"SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = '{tableName}');";
            var result = migrationBuilder.Sql(command).ToString();
            return result == "True";
        }

        private bool ColumnExists(MigrationBuilder migrationBuilder, string tableName, string columnName)
        {
            var command = $"SELECT EXISTS (SELECT FROM information_schema.columns WHERE table_name='{tableName}' AND column_name='{columnName}');";
            var result = migrationBuilder.Sql(command).ToString();
            return result == "True";
        }
    }
}
