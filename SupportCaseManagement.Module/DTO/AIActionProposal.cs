namespace SupportCaseManagement.Module.DTO
{
    public class AIActionProposal
    {
        public string Summary { get; set; } = "";
        public List<string> NextSteps { get; set; } = new();

        // Suggested field values
        public string? SuggestedPriority { get; set; }   // "P1" | "P2" | "P3"
        public string? SuggestedStatus { get; set; }     // matches CaseStatus enum name
        public string? AssignTeam { get; set; }          // raw string from AI

        // Resolved DB entities for team
        public Guid? MatchedTeamId { get; set; }         // SupportTeam.ID
        public string? MatchedTeamName { get; set; }     // SupportTeam.Name

        // KB articles — raw names from AI + matched DB records
        public List<string> SuggestedKBArticle { get; set; } = new();
        public List<KBMatch> MatchedKBArticles { get; set; } = new();  // articles found in DB

        public string? Reasoning { get; set; }
        public string? ProposedCustomerMessage { get; set; }
    }


    public class KBMatch
    {
        public Guid Id { get; set; }          // KnowledgeBaseArticle.ID (BaseObject.ID)
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
    }
}