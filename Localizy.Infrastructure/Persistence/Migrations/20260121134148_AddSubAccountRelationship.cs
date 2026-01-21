using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Localizy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSubAccountRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentBusinessId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ParentBusinessId",
                table: "Users",
                column: "ParentBusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ParentBusinessId",
                table: "Users",
                column: "ParentBusinessId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ParentBusinessId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ParentBusinessId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ParentBusinessId",
                table: "Users");
        }
    }
}
