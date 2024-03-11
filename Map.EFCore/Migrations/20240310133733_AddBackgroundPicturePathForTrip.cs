using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Map.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddBackgroundPicturePathForTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundPath",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundPath",
                table: "Trips");
        }
    }
}
