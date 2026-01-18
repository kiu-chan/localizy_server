using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Localizy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeAddressIdNullableAndAddDocumentPaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AppointmentTimeSlot",
                table: "Validations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressProofFileName",
                table: "Validations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressProofPath",
                table: "Validations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdDocumentFileName",
                table: "Validations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdDocumentPath",
                table: "Validations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressProofFileName",
                table: "Validations");

            migrationBuilder.DropColumn(
                name: "AddressProofPath",
                table: "Validations");

            migrationBuilder.DropColumn(
                name: "IdDocumentFileName",
                table: "Validations");

            migrationBuilder.DropColumn(
                name: "IdDocumentPath",
                table: "Validations");

            migrationBuilder.AlterColumn<string>(
                name: "AppointmentTimeSlot",
                table: "Validations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
