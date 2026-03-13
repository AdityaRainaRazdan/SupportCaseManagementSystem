using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTeamsInverseNav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionPolicyUser_SupportTeams_SupportTeamID",
                table: "PermissionPolicyUser");

            migrationBuilder.DropIndex(
                name: "IX_PermissionPolicyUser_SupportTeamID",
                table: "PermissionPolicyUser");

            migrationBuilder.DropColumn(
                name: "SupportTeamID",
                table: "PermissionPolicyUser");

            migrationBuilder.CreateTable(
                name: "SupportTeamMembers",
                columns: table => new
                {
                    MembersID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamsID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTeamMembers", x => new { x.MembersID, x.TeamsID });
                    table.ForeignKey(
                        name: "FK_SupportTeamMembers_PermissionPolicyUser_MembersID",
                        column: x => x.MembersID,
                        principalTable: "PermissionPolicyUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupportTeamMembers_SupportTeams_TeamsID",
                        column: x => x.TeamsID,
                        principalTable: "SupportTeams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTeamMembers_TeamsID",
                table: "SupportTeamMembers",
                column: "TeamsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportTeamMembers");

            migrationBuilder.AddColumn<Guid>(
                name: "SupportTeamID",
                table: "PermissionPolicyUser",
                type: "uniqueidentifier",
                nullable: true);

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
        }
    }
}
