using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class supportaichatsession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "SessionID",
                table: "AIInteractionLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIAssistantSession", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIInteractionLogs_SessionID",
                table: "AIInteractionLogs",
                column: "SessionID");

            migrationBuilder.AddForeignKey(
                name: "FK_AIInteractionLogs_AIAssistantSession_SessionID",
                table: "AIInteractionLogs",
                column: "SessionID",
                principalTable: "AIAssistantSession",
                principalColumn: "ID");
        }
    }
}
