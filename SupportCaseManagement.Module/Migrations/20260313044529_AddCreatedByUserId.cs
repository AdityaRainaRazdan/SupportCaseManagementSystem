using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "SupportCases",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "CaseComments",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SupportCases");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "CaseComments");
        }
    }
}
