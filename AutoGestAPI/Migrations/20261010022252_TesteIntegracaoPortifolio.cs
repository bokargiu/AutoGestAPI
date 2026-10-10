using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoGestAPI.Migrations
{
    /// <inheritdoc />
    public partial class TesteIntegracaoPortifolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_User_UserId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_UserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_User_UserId",
                table: "Service");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Service_UserId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Order_UserId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Client_UserId",
                table: "Client");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Service_UserId",
                table: "Service",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_UserId",
                table: "Client",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_User_UserId",
                table: "Client",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_User_UserId",
                table: "Service",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
