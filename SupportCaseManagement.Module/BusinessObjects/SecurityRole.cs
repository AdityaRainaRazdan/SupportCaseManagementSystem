using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using SupportCaseManagement.Module.AIBackend;
using SupportCaseManagement.Module.BusinessObjects;

namespace SupportCaseManagement.Module.Security
{
    public static class SecurityRoles
    {
        public const string Agent = "Agent";
        public const string Requester = "Requester";

        public static void ResyncRolePermissions(IObjectSpace os)
        {
            SyncAgent(os);
            SyncRequester(os);
            os.CommitChanges();
        }

        private static void SyncAgent(IObjectSpace os)
        {
            // Find existing role or create fresh
            var role = os.GetObjectsQuery<PermissionPolicyRole>()
                         .FirstOrDefault(r => r.Name == Agent);

            if (role == null)
            {
                // First time — create and set all permissions
                role = os.CreateObject<PermissionPolicyRole>();
                role.Name = Agent;
                role.IsAdministrative = false;

                role.AddTypePermission<SupportCase>(SecurityOperations.Read, SecurityPermissionState.Allow);
                role.AddTypePermission<SupportCase>(SecurityOperations.Write, SecurityPermissionState.Allow);
                role.AddTypePermission<SupportCase>(SecurityOperations.Create, SecurityPermissionState.Allow);
                role.AddTypePermission<SupportCase>(SecurityOperations.Navigate, SecurityPermissionState.Allow);

                role.AddTypePermission<CaseComment>(SecurityOperations.Read, SecurityPermissionState.Allow);
                role.AddTypePermission<CaseComment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                role.AddTypePermission<CaseComment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                role.AddTypePermission<CaseComment>(SecurityOperations.Navigate, SecurityPermissionState.Allow);

                role.AddTypePermission<CaseActionHistory>(SecurityOperations.Read, SecurityPermissionState.Allow);
                role.AddTypePermission<CaseActionHistory>(SecurityOperations.Write, SecurityPermissionState.Allow);
                role.AddTypePermission<CaseActionHistory>(SecurityOperations.Create, SecurityPermissionState.Allow);
                role.AddTypePermission<CaseActionHistory>(SecurityOperations.Navigate, SecurityPermissionState.Allow);

                role.AddTypePermission<CaseKnowledgeLink>(SecurityOperations.Read, SecurityPermissionState.Allow);
                role.AddTypePermission<CaseKnowledgeLink>(SecurityOperations.Write, SecurityPermissionState.Allow);
                role.AddTypePermission<CaseKnowledgeLink>(SecurityOperations.Create, SecurityPermissionState.Allow);
                role.AddTypePermission<CaseKnowledgeLink>(SecurityOperations.Navigate, SecurityPermissionState.Allow);

                role.AddTypePermission<KnowledgeBaseArticle>(SecurityOperations.Read, SecurityPermissionState.Allow);
                role.AddTypePermission<KnowledgeBaseArticle>(SecurityOperations.Navigate, SecurityPermissionState.Allow);

                role.AddTypePermission<AIInteractionLog>(SecurityOperations.Read, SecurityPermissionState.Allow);
                role.AddTypePermission<AIInteractionLog>(SecurityOperations.Navigate, SecurityPermissionState.Allow);

                role.AddTypePermission<SupportTeam>(SecurityOperations.Read, SecurityPermissionState.Allow);
                role.AddTypePermission<SupportTeam>(SecurityOperations.Navigate, SecurityPermissionState.Allow);
            }
            // If role already exists — leave permissions as-is.
            // To change permissions: delete the Agent role from DB and restart.
        }

        private static void SyncRequester(IObjectSpace os)
        {
            var role = os.GetObjectsQuery<PermissionPolicyRole>()
                         .FirstOrDefault(r => r.Name == Requester);

            if (role == null)
            {
                role = os.CreateObject<PermissionPolicyRole>();
                role.Name = Requester;
                role.IsAdministrative = false;

                // SupportCase — create + own cases only
                role.AddTypePermission<SupportCase>(SecurityOperations.Create, SecurityPermissionState.Allow);
                role.AddTypePermission<SupportCase>(SecurityOperations.Navigate, SecurityPermissionState.Allow);
                role.AddObjectPermission<SupportCase>(SecurityOperations.Read,
                    "[CreatedBy] = CurrentUserName()", SecurityPermissionState.Allow);
                role.AddObjectPermission<SupportCase>(SecurityOperations.Write,
                    "[CreatedBy] = CurrentUserName()", SecurityPermissionState.Allow);

                // CaseComment — public comments on own cases only
                role.AddTypePermission<CaseComment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                role.AddTypePermission<CaseComment>(SecurityOperations.Navigate, SecurityPermissionState.Allow);
                role.AddObjectPermission<CaseComment>(SecurityOperations.Read,
                    "[Case.CreatedBy] = CurrentUserName() And [CommentTypes] = 0",
                    SecurityPermissionState.Allow);
                role.AddObjectPermission<CaseComment>(SecurityOperations.Write,
                    "[Case.CreatedBy] = CurrentUserName() And [CommentTypes] = 0",
                    SecurityPermissionState.Allow);
            }
        }
    }
}