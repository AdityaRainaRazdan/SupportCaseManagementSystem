using SupportCaseManagement.Module.BusinessObjects;
using SupportCaseManagement.Module.AI;

namespace SupportCaseManagement.Blazor.Services
{
    public class AIService
    {
        public async Task<AIActionProposal> AnalyzeCaseAsync(SupportCase supportCase)
        {
            var proposal = new AIActionProposal();

            if (supportCase.Description?.ToLower().Contains("login") == true)
            {
                proposal.SuggestedPriority = "P2";
                proposal.SuggestedStatus = "InProgress";
                proposal.SuggestedTeam = "Support Team";
                proposal.SuggestedKBArticle = "Authentication Issues Guide";
                proposal.Explanation =
                    "The case appears related to authentication problems. Similar cases usually require escalation.";
            }

            return await Task.FromResult(proposal);
        }
    }
}