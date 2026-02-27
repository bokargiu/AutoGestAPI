using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoGestAPI.Migrations
{
    /// <inheritdoc />
    public partial class Ratingfoiadicionadoaosclients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Client",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Client");
        }
    }
}
