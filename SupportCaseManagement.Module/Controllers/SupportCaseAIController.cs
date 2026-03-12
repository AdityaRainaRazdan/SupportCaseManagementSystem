//using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.Actions;
//using DevExpress.Persistent.Base;
//using SupportCaseManagement.Module.AI;
//using SupportCaseManagement.Module.BusinessObjects;
//using SupportCaseManagement.Module.DTO;
//using SupportCaseManagement.Module.Services;

//namespace SupportCaseManagement.Module.Controllers
//{
//    public class SupportCaseAIController : ObjectViewController<DetailView, SupportCase>
//    {
//        private AIActionProposal proposal;

//        public SupportCaseAIController()
//        {
//            var analyzeAction = new SimpleAction(
//                this,
//                "AnalyzeCaseAI",
//                PredefinedCategory.View
//            );

//            analyzeAction.Caption = "🤖 Analyze Case";
//            analyzeAction.Execute += AnalyzeAction_Execute;
//        }

//        private async void AnalyzeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
//        {
//            var supportCase = View.CurrentObject as SupportCase;

//            var aiService = new AgenticAIService();

//            proposal = await aiService.AnalyzeCase(supportCase);

//            Application.ShowViewStrategy.ShowMessage(
//$@"AI SUMMARY

//{proposal.Summary}

//SUGGESTED PLAN

//Priority: {proposal.SuggestedPriority}
//Status: {proposal.SuggestedStatus}
//Team: {proposal.AssignTeam}

//REASON

//{proposal.Reasoning}

//Click 'Apply AI Plan' to update the case."
//            );
//        }
//    }
//}