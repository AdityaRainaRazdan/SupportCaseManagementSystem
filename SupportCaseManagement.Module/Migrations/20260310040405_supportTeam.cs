using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class supportTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTeam",
                table: "SupportCases");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedTeamID",
                table: "SupportCases",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupportTeamID",
                table: "PermissionPolicyUser",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SupportTeams",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTeams", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportCases_AssignedTeamID",
                table: "SupportCases",
                column: "AssignedTeamID");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyUser_SupportTeamID",
                table: "PermissionPolicyUser",
                column: "SupportTeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionPolicyUser_SupportTeams_SupportTeamID",
                table: "PermissionPolicyUser",
                column: "SupportTeamID",
                principalTable: "SupportTeams",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportCases_SupportTeams_AssignedTeamID",
                table: "SupportCases",
                column: "AssignedTeamID",
                principalTable: "SupportTeams",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionPolicyUser_SupportTeams_SupportTeamID",
                table: "PermissionPolicyUser");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportCases_SupportTeams_AssignedTeamID",
                table: "SupportCases");

            migrationBuilder.DropTable(
                name: "SupportTeams");

            migrationBuilder.DropIndex(
                name: "IX_SupportCases_AssignedTeamID",
                table: "SupportCases");

            migrationBuilder.DropIndex(
                name: "IX_PermissionPolicyUser_SupportTeamID",
                table: "PermissionPolicyUser");

            migrationBuilder.DropColumn(
                name: "AssignedTeamID",
                table: "SupportCases");

            migrationBuilder.DropColumn(
                name: "SupportTeamID",
                table: "PermissionPolicyUser");

            migrationBuilder.AddColumn<string>(
                name: "AssignedTeam",
                table: "SupportCases",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
