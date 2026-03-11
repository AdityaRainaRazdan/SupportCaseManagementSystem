using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class AddChatFieldsToAIInteractionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('AIInteractionLogs', 'UserMessage') IS NULL
BEGIN
ALTER TABLE AIInteractionLogs ADD UserMessage nvarchar(max) NULL
END
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('AIInteractionLogs', 'AIResponse') IS NULL
BEGIN
ALTER TABLE AIInteractionLogs ADD AIResponse nvarchar(max) NULL
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
