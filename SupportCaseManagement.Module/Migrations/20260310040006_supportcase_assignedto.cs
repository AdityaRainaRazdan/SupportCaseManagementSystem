using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class supportcase_assignedto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "SupportCases");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedToID",
                table: "SupportCases",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportCases_AssignedToID",
                table: "SupportCases",
                column: "AssignedToID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportCases_PermissionPolicyUser_AssignedToID",
                table: "SupportCases",
                column: "AssignedToID",
                principalTable: "PermissionPolicyUser",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportCases_PermissionPolicyUser_AssignedToID",
                table: "SupportCases");

            migrationBuilder.DropIndex(
                name: "IX_SupportCases_AssignedToID",
                table: "SupportCases");

            migrationBuilder.DropColumn(
                name: "AssignedToID",
                table: "SupportCases");

            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "SupportCases",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
