using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using SupportCaseManagement.Module.BusinessObjects;

namespace SupportCaseManagement.Module.Security
{
    public static class SecurityRoles
    {
        public const string Admin = "Admin";
        public const string Agent = "Agent";
        public const string Requester = "Requester";

        public static void SeedRoles(IObjectSpace objectSpace)
        {
            // Admin Role
            if (!objectSpace.GetObjectsQuery<PermissionPolicyRole>().Any(r => r.Name == Admin))
            {
                var roleAdmin = objectSpace.CreateObject<PermissionPolicyRole>();
                roleAdmin.Name = Admin;
                roleAdmin.IsAdministrative = true;
            }

            // Agent Role
            if (!objectSpace.GetObjectsQuery<PermissionPolicyRole>().Any(r => r.Name == Agent))
            {
                var roleAgent = objectSpace.CreateObject<PermissionPolicyRole>();
                roleAgent.Name = Agent;

                // Grant full access to CaseComments and KnowledgeBaseArticle
                roleAgent.AddTypePermission<SupportCase>(SecurityOperations.Read, SecurityPermissionState.Allow);
                roleAgent.AddTypePermission<SupportCase>(SecurityOperations.Write, SecurityPermissionState.Allow);
                roleAgent.AddTypePermission<CaseComment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                roleAgent.AddTypePermission<KnowledgeBaseArticle>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            }

            // Requester Role
            if (!objectSpace.GetObjectsQuery<PermissionPolicyRole>().Any(r => r.Name == Requester))
            {
                var roleRequester = objectSpace.CreateObject<PermissionPolicyRole>();
                roleRequester.Name = Requester;

                // Requester can only create and read own cases
                roleRequester.AddTypePermission<SupportCase>(SecurityOperations.Read, SecurityPermissionState.Allow);
                roleRequester.AddTypePermission<SupportCase>(SecurityOperations.Write, SecurityPermissionState.Allow);
                roleRequester.AddTypePermission<CaseComment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            }

            objectSpace.CommitChanges();
        }
    }
}