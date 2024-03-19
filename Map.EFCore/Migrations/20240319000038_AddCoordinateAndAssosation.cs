using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Map.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddCoordinateAndAssosation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "TravelTo");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Trips",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Duration",
                table: "TravelTo",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Steps",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    CoordinateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => x.CoordinateId);
                });

            migrationBuilder.CreateTable(
                name: "CoordinateStepTravelToAssociations",
                columns: table => new
                {
                    CoordinateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TravelToId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoordinateStepTravelToAssociations", x => new { x.CoordinateId, x.StepId, x.TravelToId });
                    table.ForeignKey(
                        name: "FK_CoordinateStepTravelToAssociations_Coordinates_CoordinateId",
                        column: x => x.CoordinateId,
                        principalTable: "Coordinates",
                        principalColumn: "CoordinateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoordinateStepTravelToAssociations_Steps_StepId",
                        column: x => x.StepId,
                        principalTable: "Steps",
                        principalColumn: "StepId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoordinateStepTravelToAssociations_TravelTo_TravelToId",
                        column: x => x.TravelToId,
                        principalTable: "TravelTo",
                        principalColumn: "TravelToId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoordinateStepTravelToAssociations_CoordinateId",
                table: "CoordinateStepTravelToAssociations",
                column: "CoordinateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoordinateStepTravelToAssociations_StepId",
                table: "CoordinateStepTravelToAssociations",
                column: "StepId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoordinateStepTravelToAssociations_TravelToId",
                table: "CoordinateStepTravelToAssociations",
                column: "TravelToId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoordinateStepTravelToAssociations");

            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "TravelTo");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TravelTo",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Steps",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
