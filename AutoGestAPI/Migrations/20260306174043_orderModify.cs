using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoGestAPI.Migrations
{
    /// <inheritdoc />
    public partial class orderModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Order");

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "Order",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Start",
                table: "Order",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "Order");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Day",
                table: "Order",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "Order",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "Order",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
