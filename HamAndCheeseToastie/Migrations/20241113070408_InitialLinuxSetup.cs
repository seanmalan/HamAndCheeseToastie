﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HamAndCheeseToastie.Migrations
{
    public partial class InitialLinuxSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cashier table
            migrationBuilder.Sql(@"
            DO $$
            BEGIN
                IF NOT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'Cashier') THEN
                    CREATE TABLE ""Cashier"" (
                        id integer GENERATED BY DEFAULT AS IDENTITY,
                        cashier_id integer NOT NULL,
                        first_name text NOT NULL,
                        last_name text NOT NULL,
                        employee_code text NOT NULL,
                        ""RoleId"" integer NOT NULL,
                        CONSTRAINT ""PK_Cashier"" PRIMARY KEY (id)
                    );
                END IF;
            END
            $$;
            ");

            // Categories table
            migrationBuilder.Sql(@"
            DO $$
            BEGIN
                IF NOT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'Categories') THEN
                    CREATE TABLE ""Categories"" (
                        Id integer GENERATED BY DEFAULT AS IDENTITY,
                        Name text NOT NULL,
                        CONSTRAINT ""PK_Categories"" PRIMARY KEY (Id)
                    );
                END IF;
            END
            $$;
            ");

            // Customer table
            migrationBuilder.Sql(@"
            DO $$
            BEGIN
                IF NOT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'Customer') THEN
                    CREATE TABLE ""Customer"" (
                        CustomerId integer GENERATED BY DEFAULT AS IDENTITY,
                        FirstName text NOT NULL,
                        LastName text NOT NULL,
                        Email text,
                        PhoneNumber text,
                        IsLoyaltyMember boolean NOT NULL,
                        Barcode text,
                        CONSTRAINT ""PK_Customer"" PRIMARY KEY (CustomerId)
                    );
                END IF;
            END
            $$;
            ");

            // Roles table
            migrationBuilder.Sql(@"
            DO $$
            BEGIN
                IF NOT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'Roles') THEN
                    CREATE TABLE ""Roles"" (
                        id integer GENERATED BY DEFAULT AS IDENTITY,
                        Name text NOT NULL,
                        UserId integer NOT NULL,
                        CONSTRAINT ""PK_Roles"" PRIMARY KEY (id)
                    );
                END IF;
            END
            $$;
            ");

            // Users table
            migrationBuilder.Sql(@"
            DO $$
            BEGIN
                IF NOT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'Users') THEN
                    CREATE TABLE ""Users"" (
                        Id integer GENERATED BY DEFAULT AS IDENTITY,
                        Username text NOT NULL,
                        Email text NOT NULL,
                        PasswordHash text NOT NULL,
                        CreatedAt timestamp with time zone NOT NULL,
                        UpdatedAt timestamp with time zone NOT NULL,
                        Role integer NOT NULL,
                        CONSTRAINT ""PK_Users"" PRIMARY KEY (Id)
                    );
                END IF;
            END
            $$;
            ");

            // Products table
            migrationBuilder.Sql(@"
            DO $$
            BEGIN
                IF NOT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'Products') THEN
                    CREATE TABLE ""Products"" (
                        ID integer GENERATED BY DEFAULT AS IDENTITY,
                        Name text NOT NULL,
                        BrandName text NOT NULL,
                        Weight text NOT NULL,
                        Category_id integer NOT NULL,
                        CurrentStockLevel integer NOT NULL,
                        MinimumStockLevel integer NOT NULL,
                        Price numeric NOT NULL,
                        WholesalePrice numeric NOT NULL,
                        EAN13Barcode text NOT NULL,
                        ImagePath text NOT NULL,
                        CONSTRAINT ""PK_Products"" PRIMARY KEY (ID),
                        CONSTRAINT ""FK_Products_Categories_Category_id"" FOREIGN KEY (Category_id) REFERENCES ""Categories""(Id) ON DELETE CASCADE
                    );
                END IF;
            END
            $$;
            ");

            // Transaction table
            migrationBuilder.Sql(@"
            DO $$
            BEGIN
                IF NOT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'Transaction') THEN
                    CREATE TABLE ""Transaction"" (
                        TransactionId integer GENERATED BY DEFAULT AS IDENTITY,
                        TransactionDate timestamp with time zone NOT NULL,
                        TotalAmount numeric NOT NULL,
                        Discount numeric NOT NULL,
                        PaymentMethod text NOT NULL,
                        TaxAmount numeric NOT NULL,
                        CashierId integer NOT NULL,
                        CustomerId integer NOT NULL,
                        CONSTRAINT ""PK_Transaction"" PRIMARY KEY (TransactionId),
                        CONSTRAINT ""FK_Transaction_Cashier_CashierId"" FOREIGN KEY (CashierId) REFERENCES ""Cashier""(id) ON DELETE CASCADE,
                        CONSTRAINT ""FK_Transaction_Customer_CustomerId"" FOREIGN KEY (CustomerId) REFERENCES ""Customer""(CustomerId) ON DELETE CASCADE
                    );
                END IF;
            END
            $$;
            ");

            // TransactionItem table
            migrationBuilder.Sql(@"
            DO $$
            BEGIN
                IF NOT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'TransactionItem') THEN
                    CREATE TABLE ""TransactionItem"" (
                        id integer GENERATED BY DEFAULT AS IDENTITY,
                        TransactionId integer NOT NULL,
                        ProductId integer NOT NULL,
                        Quantity integer NOT NULL,
                        UnitPrice numeric NOT NULL,
                        TotalPrice numeric NOT NULL,
                        CONSTRAINT ""PK_TransactionItem"" PRIMARY KEY (id),
                        CONSTRAINT ""FK_TransactionItem_Products_ProductId"" FOREIGN KEY (ProductId) REFERENCES ""Products""(ID) ON DELETE CASCADE,
                        CONSTRAINT ""FK_TransactionItem_Transaction_TransactionId"" FOREIGN KEY (TransactionId) REFERENCES ""Transaction""(TransactionId) ON DELETE CASCADE
                    );
                END IF;
            END
            $$;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Roles");
            migrationBuilder.DropTable(name: "TransactionItem");
            migrationBuilder.DropTable(name: "Users");
            migrationBuilder.DropTable(name: "Products");
            migrationBuilder.DropTable(name: "Transaction");
            migrationBuilder.DropTable(name: "Categories");
            migrationBuilder.DropTable(name: "Cashier");
            migrationBuilder.DropTable(name: "Customer");
        }
    }
}
