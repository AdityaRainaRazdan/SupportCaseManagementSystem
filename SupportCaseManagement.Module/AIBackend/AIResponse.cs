namespace SupportCaseManagement.Module.AIBackend
{
    public class AIResponse
    {
        public string summary { get; set; } = "";
        public List<string> next_steps { get; set; } = new();
        public AIPlan plan { get; set; } = new();
        public string reasoning { get; set; } = "";
        public string proposed_message { get; set; } = "";
    }

    public class AIPlan
    {
        public string? priority { get; set; }       // "P1" | "P2" | "P3"
        public string? status { get; set; }          // "New" | "Triage" | "InProgress" | "WaitingCustomer" | "Resolved" | "Closed"
        public string? assign_team { get; set; }     // must match SupportTeam.Name
        public List<string> kb_articles { get; set; } = new(); // must match KnowledgeBaseArticle.Title
    }

}