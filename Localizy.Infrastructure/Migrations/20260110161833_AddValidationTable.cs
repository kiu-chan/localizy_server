using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Localizy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddValidationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Validations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OldData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PhotosProvided = table.Column<bool>(type: "bit", nullable: false),
                    DocumentsProvided = table.Column<bool>(type: "bit", nullable: false),
                    LocationVerified = table.Column<bool>(type: "bit", nullable: false),
                    AttachmentsCount = table.Column<int>(type: "int", nullable: false),
                    ProcessedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessingNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Validations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Validations_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Validations_Users_ProcessedByUserId",
                        column: x => x.ProcessedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Validations_Users_SubmittedByUserId",
                        column: x => x.SubmittedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Validations_AddressId",
                table: "Validations",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Validations_Priority",
                table: "Validations",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Validations_ProcessedByUserId",
                table: "Validations",
                column: "ProcessedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Validations_RequestId",
                table: "Validations",
                column: "RequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Validations_Status",
                table: "Validations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Validations_SubmittedByUserId",
                table: "Validations",
                column: "SubmittedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Validations_SubmittedDate",
                table: "Validations",
                column: "SubmittedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Validations");
        }
    }
}
