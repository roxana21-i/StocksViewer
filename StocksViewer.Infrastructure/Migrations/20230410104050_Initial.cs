using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StocksViewer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BuyOrders",
                columns: table => new
                {
                    BuyOrderID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StockSymbol = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StockName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateAndTimeOfOrder = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Quantity = table.Column<uint>(type: "int unsigned", nullable: false),
                    Price = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyOrders", x => x.BuyOrderID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SellOrders",
                columns: table => new
                {
                    SellOrderID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StockSymbol = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StockName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateAndTimeOfOrder = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Quantity = table.Column<uint>(type: "int unsigned", nullable: false),
                    Price = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellOrders", x => x.SellOrderID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "BuyOrders",
                columns: new[] { "BuyOrderID", "DateAndTimeOfOrder", "Price", "Quantity", "StockName", "StockSymbol" },
                values: new object[,]
                {
                    { new Guid("6081ce3b-7a4b-4fb3-9342-01276fec3540"), new DateTime(2023, 3, 21, 12, 15, 3, 0, DateTimeKind.Unspecified), 210.0, 10u, "Microsoft Corporation", "MSFT" },
                    { new Guid("6aa989b4-c641-42ee-8f56-e5db5cd44f45"), new DateTime(2023, 3, 22, 16, 21, 12, 0, DateTimeKind.Unspecified), 147.0, 55u, "Apple Inc.", "AAPL" },
                    { new Guid("ea461269-cea8-4c01-9a43-bbcc39a68953"), new DateTime(2023, 3, 22, 12, 10, 0, 0, DateTimeKind.Unspecified), 237.0, 100u, "Microsoft Corporation", "MSFT" },
                    { new Guid("fe154c85-d418-4bcd-adce-abe4b5520a84"), new DateTime(2023, 3, 22, 19, 55, 22, 0, DateTimeKind.Unspecified), 134.0, 400u, "Amazon.com, Inc", "AMZN" }
                });

            migrationBuilder.InsertData(
                table: "SellOrders",
                columns: new[] { "SellOrderID", "DateAndTimeOfOrder", "Price", "Quantity", "StockName", "StockSymbol" },
                values: new object[,]
                {
                    { new Guid("002a4233-9e72-42fd-87b2-ac0cba198a75"), new DateTime(2023, 3, 22, 5, 11, 45, 0, DateTimeKind.Unspecified), 150.0, 40u, "Apple Inc.", "AAPL" },
                    { new Guid("a1ec73fb-2b1e-470e-a50e-c3ae5031393c"), new DateTime(2023, 3, 22, 15, 12, 0, 0, DateTimeKind.Unspecified), 200.0, 5u, "Microsoft Corporation", "MSFT" },
                    { new Guid("cb600cef-05f5-4bcc-ade5-508cbe50ae0c"), new DateTime(2023, 3, 22, 3, 24, 0, 0, DateTimeKind.Unspecified), 250.0, 99u, "Microsoft Corporation", "MSFT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyOrders");

            migrationBuilder.DropTable(
                name: "SellOrders");
        }
    }
}
