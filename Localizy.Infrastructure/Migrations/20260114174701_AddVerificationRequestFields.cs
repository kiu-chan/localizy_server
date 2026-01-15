using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Localizy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVerificationRequestFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDate",
                table: "Validations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppointmentTimeSlot",
                table: "Validations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdType",
                table: "Validations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Validations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Validations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentAmount",
                table: "Validations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Validations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Validations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "Validations");

            migrationBuilder.DropColumn(
                name: "AppointmentTimeSlot",
                table: "Validations");

            migrationBuilder.DropColumn(
                name: "IdType",
                table: "Validations");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Validations");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Validations");

            migrationBuilder.DropColumn(
                name: "PaymentAmount",
                table: "Validations");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Validations");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Validations");
        }
    }
}
