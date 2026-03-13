using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class currentusername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIChatMessages");

            migrationBuilder.DropTable(
                name: "supportCaseAIChatSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "supportCaseAIChatSessions",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelatedCaseID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SessionName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supportCaseAIChatSessions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_supportCaseAIChatSessions_SupportCases_RelatedCaseID",
                        column: x => x.RelatedCaseID,
                        principalTable: "SupportCases",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AIChatMessages",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsAI = table.Column<bool>(type: "bit", nullable: false),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIChatMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AIChatMessages_supportCaseAIChatSessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "supportCaseAIChatSessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIChatMessages_SessionID",
                table: "AIChatMessages",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_supportCaseAIChatSessions_RelatedCaseID",
                table: "supportCaseAIChatSessions",
                column: "RelatedCaseID");
        }
    }
}
