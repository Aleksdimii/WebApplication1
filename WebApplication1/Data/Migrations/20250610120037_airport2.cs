using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Data.Migrations
{
    /// <inheritdoc />
    public partial class airport2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "Airports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airports_FlightId",
                table: "Airports",
                column: "FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Airports_Flights_FlightId",
                table: "Airports",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airports_Flights_FlightId",
                table: "Airports");

            migrationBuilder.DropIndex(
                name: "IX_Airports_FlightId",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Airports");
        }
    }
}
