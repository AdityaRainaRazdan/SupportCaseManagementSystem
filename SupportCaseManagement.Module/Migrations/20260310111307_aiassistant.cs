using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class aiassistant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AIAssistantSession_PermissionPolicyUser_CreatedByID",
                table: "AIAssistantSession");

            migrationBuilder.DropForeignKey(
                name: "FK_AIInteractionLogs_AIAssistantSession_SessionID",
                table: "AIInteractionLogs");

            migrationBuilder.DropIndex(
                name: "IX_AIAssistantSession_CreatedByID",
                table: "AIAssistantSession");

            migrationBuilder.DropColumn(
                name: "CreatedByID",
                table: "AIAssistantSession");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AIAssistantSession");

            migrationBuilder.AddForeignKey(
                name: "FK_AIInteractionLogs_AIAssistantSession_SessionID",
                table: "AIInteractionLogs",
                column: "SessionID",
                principalTable: "AIAssistantSession",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AIInteractionLogs_AIAssistantSession_SessionID",
                table: "AIInteractionLogs");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByID",
                table: "AIAssistantSession",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AIAssistantSession",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_AIAssistantSession_CreatedByID",
                table: "AIAssistantSession",
                column: "CreatedByID");

            migrationBuilder.AddForeignKey(
                name: "FK_AIAssistantSession_PermissionPolicyUser_CreatedByID",
                table: "AIAssistantSession",
                column: "CreatedByID",
                principalTable: "PermissionPolicyUser",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AIInteractionLogs_AIAssistantSession_SessionID",
                table: "AIInteractionLogs",
                column: "SessionID",
                principalTable: "AIAssistantSession",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
