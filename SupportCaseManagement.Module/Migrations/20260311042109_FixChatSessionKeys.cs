using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class FixChatSessionKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AIChatMessages_supportCaseAIChatSessions_SessionID",
                table: "AIChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_supportCaseAIChatSessions",
                table: "supportCaseAIChatSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AIChatMessages",
                table: "AIChatMessages");

            migrationBuilder.RenameColumn(
    name: "Id",
    table: "supportCaseAIChatSessions",
    newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AIChatMessages",
                newName: "ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "supportCaseAIChatSessions",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "ID",
                table: "AIChatMessages",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_supportCaseAIChatSessions",
                table: "supportCaseAIChatSessions",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AIChatMessages",
                table: "AIChatMessages",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AIChatMessages_supportCaseAIChatSessions_SessionID",
                table: "AIChatMessages",
                column: "SessionID",
                principalTable: "supportCaseAIChatSessions",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AIChatMessages_supportCaseAIChatSessions_SessionID",
                table: "AIChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_supportCaseAIChatSessions",
                table: "supportCaseAIChatSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AIChatMessages",
                table: "AIChatMessages");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "supportCaseAIChatSessions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "AIChatMessages",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "supportCaseAIChatSessions",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ID",
                table: "supportCaseAIChatSessions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AIChatMessages",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ID",
                table: "AIChatMessages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_supportCaseAIChatSessions",
                table: "supportCaseAIChatSessions",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AIChatMessages",
                table: "AIChatMessages",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AIChatMessages_supportCaseAIChatSessions_SessionID",
                table: "AIChatMessages",
                column: "SessionID",
                principalTable: "supportCaseAIChatSessions",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
