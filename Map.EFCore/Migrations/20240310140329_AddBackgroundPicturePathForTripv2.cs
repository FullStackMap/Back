using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Map.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddBackgroundPicturePathForTripv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BackgroundPath",
                table: "Trips",
                newName: "BackgroundPicturePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BackgroundPicturePath",
                table: "Trips",
                newName: "BackgroundPath");
        }
    }
}
