using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Map.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class TripIdInTravels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "rate",
                table: "Testimonials",
                newName: "Rate");

            migrationBuilder.AddColumn<Guid>(
                name: "TripId",
                table: "Travels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Travels_TripId",
                table: "Travels",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Travels_Trips_TripId",
                table: "Travels",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "TripId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Travels_Trips_TripId",
                table: "Travels");

            migrationBuilder.DropIndex(
                name: "IX_Travels_TripId",
                table: "Travels");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Travels");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "Testimonials",
                newName: "rate");
        }
    }
}
