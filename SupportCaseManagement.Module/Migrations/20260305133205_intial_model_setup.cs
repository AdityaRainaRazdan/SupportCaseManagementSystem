using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportCaseManagement.Module.Migrations
{
    /// <inheritdoc />
    public partial class intial_model_setup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KnowledgeBaseArticles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeBaseArticles", x => x.ID);
                });

            

            //migrationBuilder.CreateTable(
            //    name: "PermissionPolicyRoleBase",
            //    columns: table => new
            //    {
            //        ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsAdministrative = table.Column<bool>(type: "bit", nullable: false),
            //        CanEditModel = table.Column<bool>(type: "bit", nullable: false),
            //        PermissionPolicy = table.Column<int>(type: "int", nullable: false),
            //        IsAllowPermissionPriority = table.Column<bool>(type: "bit", nullable: false),
            //        Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
            //        GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
            //        OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermissionPolicyRoleBase", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PermissionPolicyUser",
            //    columns: table => new
            //    {
            //        ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        ChangePasswordOnFirstLogon = table.Column<bool>(type: "bit", nullable: false),
            //        StoredPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
            //        AccessFailedCount = table.Column<int>(type: "int", nullable: true),
            //        LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
            //        OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermissionPolicyUser", x => x.ID);
            //    });

            migrationBuilder.CreateTable(
                name: "SupportCases",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssignedTo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssignedTeam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportCases", x => x.ID);
                });

            //migrationBuilder.CreateTable(
            //    name: "ModelDifferenceAspects",
            //    columns: table => new
            //    {
            //        ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Xml = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        OwnerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ModelDifferenceAspects", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_ModelDifferenceAspects_ModelDifferences_OwnerID",
            //            column: x => x.OwnerID,
            //            principalTable: "ModelDifferences",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PermissionPolicyActionPermissionObject",
            //    columns: table => new
            //    {
            //        ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        ActionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
            //        OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermissionPolicyActionPermissionObject", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_PermissionPolicyActionPermissionObject_PermissionPolicyRoleBase_RoleID",
            //            column: x => x.RoleID,
            //            principalTable: "PermissionPolicyRoleBase",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PermissionPolicyNavigationPermissionObject",
            //    columns: table => new
            //    {
            //        ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        ItemPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TargetTypeFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NavigateState = table.Column<int>(type: "int", nullable: true),
            //        GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
            //        OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermissionPolicyNavigationPermissionObject", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_PermissionPolicyNavigationPermissionObject_PermissionPolicyRoleBase_RoleID",
            //            column: x => x.RoleID,
            //            principalTable: "PermissionPolicyRoleBase",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PermissionPolicyTypePermissionObject",
            //    columns: table => new
            //    {
            //        ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TargetTypeFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        ReadState = table.Column<int>(type: "int", nullable: true),
            //        WriteState = table.Column<int>(type: "int", nullable: true),
            //        CreateState = table.Column<int>(type: "int", nullable: true),
            //        DeleteState = table.Column<int>(type: "int", nullable: true),
            //        NavigateState = table.Column<int>(type: "int", nullable: true),
            //        GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
            //        OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermissionPolicyTypePermissionObject", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_PermissionPolicyTypePermissionObject_PermissionPolicyRoleBase_RoleID",
            //            column: x => x.RoleID,
            //            principalTable: "PermissionPolicyRoleBase",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PermissionPolicyRolePermissionPolicyUser",
            //    columns: table => new
            //    {
            //        RolesID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UsersID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermissionPolicyRolePermissionPolicyUser", x => new { x.RolesID, x.UsersID });
            //        table.ForeignKey(
            //            name: "FK_PermissionPolicyRolePermissionPolicyUser_PermissionPolicyRoleBase_RolesID",
            //            column: x => x.RolesID,
            //            principalTable: "PermissionPolicyRoleBase",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_PermissionPolicyRolePermissionPolicyUser_PermissionPolicyUser_UsersID",
            //            column: x => x.UsersID,
            //            principalTable: "PermissionPolicyUser",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PermissionPolicyUserLoginInfo",
            //    columns: table => new
            //    {
            //        ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        LoginProviderName = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        ProviderUserKey = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        UserForeignKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
            //        OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermissionPolicyUserLoginInfo", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_PermissionPolicyUserLoginInfo_PermissionPolicyUser_UserForeignKey",
            //            column: x => x.UserForeignKey,
            //            principalTable: "PermissionPolicyUser",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateTable(
                name: "AIInteractionLogs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AIResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutedPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIInteractionLogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AIInteractionLogs_SupportCases_SupportCaseId",
                        column: x => x.SupportCaseId,
                        principalTable: "SupportCases",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseActionHistories",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PerformedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseActionHistories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CaseActionHistories_SupportCases_SupportCaseId",
                        column: x => x.SupportCaseId,
                        principalTable: "SupportCases",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseComments",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseComments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CaseComments_SupportCases_SupportCaseId",
                        column: x => x.SupportCaseId,
                        principalTable: "SupportCases",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseKnowledgeLinks",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KnowledgeBaseArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseKnowledgeLinks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CaseKnowledgeLinks_KnowledgeBaseArticles_KnowledgeBaseArticleId",
                        column: x => x.KnowledgeBaseArticleId,
                        principalTable: "KnowledgeBaseArticles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseKnowledgeLinks_SupportCases_SupportCaseId",
                        column: x => x.SupportCaseId,
                        principalTable: "SupportCases",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "PermissionPolicyMemberPermissionsObject",
            //    columns: table => new
            //    {
            //        ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Members = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Criteria = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ReadState = table.Column<int>(type: "int", nullable: true),
            //        WriteState = table.Column<int>(type: "int", nullable: true),
            //        TypePermissionObjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
            //        OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermissionPolicyMemberPermissionsObject", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_PermissionPolicyMemberPermissionsObject_PermissionPolicyTypePermissionObject_TypePermissionObjectID",
            //            column: x => x.TypePermissionObjectID,
            //            principalTable: "PermissionPolicyTypePermissionObject",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PermissionPolicyObjectPermissionsObject",
            //    columns: table => new
            //    {
            //        ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Criteria = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ReadState = table.Column<int>(type: "int", nullable: true),
            //        WriteState = table.Column<int>(type: "int", nullable: true),
            //        DeleteState = table.Column<int>(type: "int", nullable: true),
            //        NavigateState = table.Column<int>(type: "int", nullable: true),
            //        TypePermissionObjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        GCRecord = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
            //        OptimisticLockField = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermissionPolicyObjectPermissionsObject", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_PermissionPolicyObjectPermissionsObject_PermissionPolicyTypePermissionObject_TypePermissionObjectID",
            //            column: x => x.TypePermissionObjectID,
            //            principalTable: "PermissionPolicyTypePermissionObject",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_AIInteractionLogs_SupportCaseId",
                table: "AIInteractionLogs",
                column: "SupportCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseActionHistories_SupportCaseId",
                table: "CaseActionHistories",
                column: "SupportCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseComments_SupportCaseId",
                table: "CaseComments",
                column: "SupportCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseKnowledgeLinks_KnowledgeBaseArticleId",
                table: "CaseKnowledgeLinks",
                column: "KnowledgeBaseArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseKnowledgeLinks_SupportCaseId",
                table: "CaseKnowledgeLinks",
                column: "SupportCaseId");

            ////migrationBuilder.CreateIndex(
            ////    name: "IX_ModelDifferenceAspects_OwnerID",
            ////    table: "ModelDifferenceAspects",
            ////    column: "OwnerID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PermissionPolicyActionPermissionObject_RoleID",
            //    table: "PermissionPolicyActionPermissionObject",
            //    column: "RoleID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PermissionPolicyMemberPermissionsObject_TypePermissionObjectID",
            //    table: "PermissionPolicyMemberPermissionsObject",
            //    column: "TypePermissionObjectID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PermissionPolicyNavigationPermissionObject_RoleID",
            //    table: "PermissionPolicyNavigationPermissionObject",
            //    column: "RoleID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PermissionPolicyObjectPermissionsObject_TypePermissionObjectID",
            //    table: "PermissionPolicyObjectPermissionsObject",
            //    column: "TypePermissionObjectID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PermissionPolicyRolePermissionPolicyUser_UsersID",
            //    table: "PermissionPolicyRolePermissionPolicyUser",
            //    column: "UsersID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PermissionPolicyTypePermissionObject_RoleID",
            //    table: "PermissionPolicyTypePermissionObject",
            //    column: "RoleID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PermissionPolicyUserLoginInfo_LoginProviderName_ProviderUserKey",
            //    table: "PermissionPolicyUserLoginInfo",
            //    columns: new[] { "LoginProviderName", "ProviderUserKey" },
            //    unique: true,
            //    filter: "[LoginProviderName] IS NOT NULL AND [ProviderUserKey] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PermissionPolicyUserLoginInfo_UserForeignKey",
            //    table: "PermissionPolicyUserLoginInfo",
            //    column: "UserForeignKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIInteractionLogs");

            migrationBuilder.DropTable(
                name: "CaseActionHistories");

            migrationBuilder.DropTable(
                name: "CaseComments");

            migrationBuilder.DropTable(
                name: "CaseKnowledgeLinks");

            migrationBuilder.DropTable(
                name: "ModelDifferenceAspects");

            migrationBuilder.DropTable(
                name: "PermissionPolicyActionPermissionObject");

            migrationBuilder.DropTable(
                name: "PermissionPolicyMemberPermissionsObject");

            migrationBuilder.DropTable(
                name: "PermissionPolicyNavigationPermissionObject");

            migrationBuilder.DropTable(
                name: "PermissionPolicyObjectPermissionsObject");

            migrationBuilder.DropTable(
                name: "PermissionPolicyRolePermissionPolicyUser");

            migrationBuilder.DropTable(
                name: "PermissionPolicyUserLoginInfo");

            migrationBuilder.DropTable(
                name: "KnowledgeBaseArticles");

            migrationBuilder.DropTable(
                name: "SupportCases");

            migrationBuilder.DropTable(
                name: "ModelDifferences");

            migrationBuilder.DropTable(
                name: "PermissionPolicyTypePermissionObject");

            migrationBuilder.DropTable(
                name: "PermissionPolicyUser");

            migrationBuilder.DropTable(
                name: "PermissionPolicyRoleBase");
        }
    }
}
