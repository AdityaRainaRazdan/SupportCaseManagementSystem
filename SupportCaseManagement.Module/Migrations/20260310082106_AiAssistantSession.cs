using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class AiAssistantSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AIModel",
                table: "AIInteractionLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActionExecuted",
                table: "AIInteractionLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PlanApplied",
                table: "AIInteractionLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SessionID",
                table: "AIInteractionLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AIAssistantSession",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIAssistantSession", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AIAssistantSession_PermissionPolicyUser_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIInteractionLogs_SessionID",
                table: "AIInteractionLogs",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_AIAssistantSession_CreatedByID",
                table: "AIAssistantSession",
                column: "CreatedByID");

            migrationBuilder.AddForeignKey(
                name: "FK_AIInteractionLogs_AIAssistantSession_SessionID",
                table: "AIInteractionLogs",
                column: "SessionID",
                principalTable: "AIAssistantSession",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AIInteractionLogs_AIAssistantSession_SessionID",
                table: "AIInteractionLogs");

            migrationBuilder.DropTable(
                name: "AIAssistantSession");

            migrationBuilder.DropIndex(
                name: "IX_AIInteractionLogs_SessionID",
                table: "AIInteractionLogs");

            migrationBuilder.DropColumn(
                name: "AIModel",
                table: "AIInteractionLogs");

            migrationBuilder.DropColumn(
                name: "ActionExecuted",
                table: "AIInteractionLogs");

            migrationBuilder.DropColumn(
                name: "PlanApplied",
                table: "AIInteractionLogs");

            migrationBuilder.DropColumn(
                name: "SessionID",
                table: "AIInteractionLogs");
        }
    }
}
