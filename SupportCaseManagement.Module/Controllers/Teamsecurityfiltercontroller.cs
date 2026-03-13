//using DevExpress.Data.Filtering;
//using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.Security;
//using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
//using Microsoft.EntityFrameworkCore;
//using SupportCaseManagement.Module.BusinessObjects;
//using SupportCaseManagement.Module.AIBackend;

//namespace SupportCaseManagement.Module.Controllers
//{
//    public class TeamSecurityFilterController : ViewController<ListView>
//    {
//        // Only these types get team filtering — everything else is skipped
//        private static readonly HashSet<Type> FilteredTypes = new()
//        {
//            typeof(SupportCase),
//            typeof(CaseComment),
//            typeof(CaseActionHistory),
//            typeof(CaseKnowledgeLink),
//            typeof(AIInteractionLog)
//        };

//        protected override void OnActivated()
//        {
//            base.OnActivated();

//            var viewType = View.ObjectTypeInfo.Type;

//            // Skip immediately if this type is not in our filtered list
//            if (!FilteredTypes.Contains(viewType)) return;

//            // Admins see everything — skip
//            if (IsAdminUser()) return;

//            // Only apply team filter for Agents — Requesters are handled by
//            // XAF object-level security criteria (CurrentUserName()) only
//            if (!IsAgentUser()) return;

//            var teamIds = GetCurrentUserTeamIds();

//            // Agent with no team assigned → sees nothing
//            if (!teamIds.Any())
//            {
//                ApplyFilter(View.CollectionSource, CriteriaOperator.Parse("1 = 0"));
//                return;
//            }

//            var teamIdObjects = teamIds.Cast<object>().ToList();
//            CriteriaOperator filter = null;

//            if (viewType == typeof(SupportCase))
//                filter = new InOperator("AssignedTeam.ID", teamIdObjects);
//            else if (viewType == typeof(CaseComment))
//                filter = new InOperator("Case.AssignedTeam.ID", teamIdObjects);
//            else if (viewType == typeof(CaseActionHistory))
//                filter = new InOperator("Case.AssignedTeam.ID", teamIdObjects);
//            else if (viewType == typeof(CaseKnowledgeLink))
//                filter = new InOperator("Case.AssignedTeam.ID", teamIdObjects);
//            else if (viewType == typeof(AIInteractionLog))
//                filter = new InOperator("Case.AssignedTeam.ID", teamIdObjects);

//            if (filter != null)
//                ApplyFilter(View.CollectionSource, filter);
//        }

//        protected override void OnDeactivated()
//        {
//            try
//            {
//                if (View?.CollectionSource != null)
//                    View.CollectionSource.Criteria.Remove("TeamSecurityFilter");
//            }
//            catch { }
//            base.OnDeactivated();
//        }

//        private void ApplyFilter(CollectionSourceBase source, CriteriaOperator criteria)
//        {
//            source.Criteria["TeamSecurityFilter"] = criteria;
//        }

//        private bool IsAdminUser()
//        {
//            try
//            {
//                using var os = Application.CreateObjectSpace(typeof(ApplicationUser));
//                var user = os.GetObjectsQuery<ApplicationUser>()
//                             .Include(u => u.Roles)
//                             .FirstOrDefault(u => u.UserName == SecuritySystem.CurrentUserName);
//                return user?.Roles?.Any(r => r.IsAdministrative) ?? false;
//            }
//            catch { return false; }
//        }

//        private bool IsAgentUser()
//        {
//            try
//            {
//                using var os = Application.CreateObjectSpace(typeof(ApplicationUser));
//                var user = os.GetObjectsQuery<ApplicationUser>()
//                             .Include(u => u.Roles)
//                             .FirstOrDefault(u => u.UserName == SecuritySystem.CurrentUserName);
//                return user?.Roles?.Any(r => r.Name == "Agent") ?? false;
//            }
//            catch { return false; }
//        }

//        private List<Guid> GetCurrentUserTeamIds()
//        {
//            try
//            {
//                var userName = SecuritySystem.CurrentUserName;
//                if (string.IsNullOrEmpty(userName)) return new();

//                using var os = Application.CreateObjectSpace(typeof(ApplicationUser));
//                var user = os.GetObjectsQuery<ApplicationUser>()
//                             .Include(u => u.Teams)
//                             .FirstOrDefault(u => u.UserName == userName);
//                return user?.Teams?.Select(t => t.ID).ToList() ?? new();
//            }
//            catch { return new(); }
//        }
//    }
//}