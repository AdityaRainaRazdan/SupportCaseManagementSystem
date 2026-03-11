using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class casecomment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CaseComments");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByID",
                table: "CaseComments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseComments_CreatedByID",
                table: "CaseComments",
                column: "CreatedByID");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseComments_PermissionPolicyUser_CreatedByID",
                table: "CaseComments",
                column: "CreatedByID",
                principalTable: "PermissionPolicyUser",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseComments_PermissionPolicyUser_CreatedByID",
                table: "CaseComments");

            migrationBuilder.DropIndex(
                name: "IX_CaseComments_CreatedByID",
                table: "CaseComments");

            migrationBuilder.DropColumn(
                name: "CreatedByID",
                table: "CaseComments");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CaseComments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
