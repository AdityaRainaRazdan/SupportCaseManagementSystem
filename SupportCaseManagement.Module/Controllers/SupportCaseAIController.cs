using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using SupportCaseManagement.Module.BusinessObjects;
using SupportCaseManagement.Blazor.Services;

namespace SupportCaseManagement.Module.Controllers
{
    public class SupportCaseAIController : ObjectViewController<DetailView, SupportCase>
    {
        public SupportCaseAIController()
        {
            var analyzeAction = new SimpleAction(
                this,
                "AnalyzeCaseAI",
                PredefinedCategory.View
            );

            analyzeAction.Caption = "🤖 Analyze Case";
            analyzeAction.Execute += AnalyzeAction_Execute;
        }

        private async void AnalyzeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var supportCase = View.CurrentObject as SupportCase;

            if (supportCase == null)
                return;

            var aiService = new AIService();

            var result = await aiService.AnalyzeCaseAsync(supportCase);

            Application.ShowViewStrategy.ShowMessage(
                $"AI Suggestion:\n\n{result.Explanation}"
            );
        }
    }
}