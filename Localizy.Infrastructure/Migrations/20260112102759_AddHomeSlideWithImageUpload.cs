using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Localizy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeSlideWithImageUpload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "HomeSlides",
                newName: "ImagePath");

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "HomeSlides",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "HomeSlides");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "HomeSlides",
                newName: "ImageUrl");
        }
    }
}
