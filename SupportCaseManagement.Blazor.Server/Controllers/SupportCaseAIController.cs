//using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.Actions;
//using SupportCaseManagement.Module.BusinessObjects;
//using SupportCaseManagement.Module.Services;

//namespace SupportCaseManagement.Blazor.Server.Controllers
//{
//    public class SupportCaseAIController : ViewController<DetailView>
//    {
//        private readonly AIService aiService;
//        private SimpleAction aiAnalyzeAction;

//        public SupportCaseAIController()
//        {
//            TargetObjectType = typeof(SupportCase);

//            aiAnalyzeAction = new SimpleAction(
//                this,
//                "AIAnalyzeCase",
//                DevExpress.Persistent.Base.PredefinedCategory.View
//            );

//            aiAnalyzeAction.Caption = "🤖 Analyze Case";
//            aiAnalyzeAction.Execute += AiAnalyzeAction_Execute;

//            aiService = new AIService();
//        }

//        private async void AiAnalyzeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
//        {
//            var supportCase = View.CurrentObject as SupportCase;

//            if (supportCase == null)
//                return;

//            var proposal = await aiService.AnalyzeCaseAsync(supportCase);

//            string message =
//                $"AI Suggestion\n\n" +
//                $"Priority → {proposal.SuggestedPriority}\n" +
//                $"Status → {proposal.SuggestedStatus}\n" +
//                $"Team → {proposal.SuggestedTeam}\n" +
//                $"KB Article → {proposal.SuggestedKBArticle}\n\n" +
//                $"{proposal.Explanation}";

//            Application.ShowViewStrategy.ShowMessage(message);
//        }
//    }
//}