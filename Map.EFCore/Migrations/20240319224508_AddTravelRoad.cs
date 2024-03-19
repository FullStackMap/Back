using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Map.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddTravelRoad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TravelTo");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Trips",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Steps",
                type: "decimal(18,12)",
                precision: 18,
                scale: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Steps",
                type: "decimal(18,12)",
                precision: 18,
                scale: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

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
                name: "TravelRoads",
                columns: table => new
                {
                    TravelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoadCoordinates = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelRoads", x => x.TravelId);
                });

            migrationBuilder.CreateTable(
                name: "Travels",
                columns: table => new
                {
                    TravelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransportMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Distance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Duration = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarbonEmition = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Travels", x => x.TravelId);
                    table.ForeignKey(
                        name: "FK_Travels_Steps_DestinationStepId",
                        column: x => x.DestinationStepId,
                        principalTable: "Steps",
                        principalColumn: "StepId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Travels_Steps_OriginStepId",
                        column: x => x.OriginStepId,
                        principalTable: "Steps",
                        principalColumn: "StepId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Travels_TravelRoads_TravelId",
                        column: x => x.TravelId,
                        principalTable: "TravelRoads",
                        principalColumn: "TravelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Travels_DestinationStepId",
                table: "Travels",
                column: "DestinationStepId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Travels_OriginStepId",
                table: "Travels",
                column: "OriginStepId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Travels");

            migrationBuilder.DropTable(
                name: "TravelRoads");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Steps",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,12)",
                oldPrecision: 18,
                oldScale: 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Steps",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,12)",
                oldPrecision: 18,
                oldScale: 12);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Steps",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TravelTo",
                columns: table => new
                {
                    TravelToId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreviousStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarbonEmition = table.Column<int>(type: "int", nullable: true),
                    Distance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TransportMode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelTo", x => x.TravelToId);
                    table.ForeignKey(
                        name: "FK_TravelTo_Steps_CurrentStepId",
                        column: x => x.CurrentStepId,
                        principalTable: "Steps",
                        principalColumn: "StepId");
                    table.ForeignKey(
                        name: "FK_TravelTo_Steps_PreviousStepId",
                        column: x => x.PreviousStepId,
                        principalTable: "Steps",
                        principalColumn: "StepId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TravelTo_CurrentStepId",
                table: "TravelTo",
                column: "CurrentStepId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelTo_PreviousStepId",
                table: "TravelTo",
                column: "PreviousStepId");
        }
    }
}
