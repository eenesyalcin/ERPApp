using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductionId",
                table: "stockMovements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Productions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_stockMovements_ProductionId",
                table: "stockMovements",
                column: "ProductionId");

            migrationBuilder.CreateIndex(
                name: "IX_Productions_ProductId",
                table: "Productions",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_stockMovements_Productions_ProductionId",
                table: "stockMovements",
                column: "ProductionId",
                principalTable: "Productions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stockMovements_Productions_ProductionId",
                table: "stockMovements");

            migrationBuilder.DropTable(
                name: "Productions");

            migrationBuilder.DropIndex(
                name: "IX_stockMovements_ProductionId",
                table: "stockMovements");

            migrationBuilder.DropColumn(
                name: "ProductionId",
                table: "stockMovements");
        }
    }
}
