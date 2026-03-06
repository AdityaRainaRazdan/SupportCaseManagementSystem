namespace SupportCaseManagement.Module.AI
{
    public class AIActionProposal
    {
        public string SuggestedPriority { get; set; }
        public string SuggestedStatus { get; set; }
        public string SuggestedTeam { get; set; }
        public string SuggestedKBArticle { get; set; }
        public string Explanation { get; set; }
    }
}