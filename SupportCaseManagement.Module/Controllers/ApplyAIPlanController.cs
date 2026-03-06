using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using SupportCaseManagement.Module.BusinessObjects;

namespace SupportCaseManagement.Module.Controllers
{
    public class ApplyAIPlanController : ObjectViewController<DetailView, SupportCase>
    {
        public ApplyAIPlanController()
        {
            var applyAction = new SimpleAction(
                this,
                "ApplyAIPlan",
                PredefinedCategory.View
            );

            applyAction.Caption = "Apply AI Plan";
            applyAction.Execute += ApplyAction_Execute;
        }

        private void ApplyAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var supportCase = View.CurrentObject as SupportCase;

            if (supportCase == null)
                return;

            supportCase.Priority = CasePriority.P1;
            supportCase.Status = CaseStatus.InProgress;

            ObjectSpace.CommitChanges();

            Application.ShowViewStrategy.ShowMessage(
                "AI plan applied successfully."
            );
        }
    }
}