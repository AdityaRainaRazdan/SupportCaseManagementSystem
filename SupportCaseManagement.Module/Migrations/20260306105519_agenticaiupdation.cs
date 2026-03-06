using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class agenticaiupdation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "CaseComments",
                newName: "CommentTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommentTypes",
                table: "CaseComments",
                newName: "Type");
        }
    }
}
