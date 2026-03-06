using System.Collections.ObjectModel;

namespace SupportCaseManagement.Module.DTO
{
    public class AIActionProposal
    {
        public virtual string Summary { get; set; }
        public virtual string SuggestedPriority { get; set; }
        public virtual string SuggestedStatus { get; set; }
        public virtual string SuggestedTeam { get; set; }
        public virtual List<string> SuggestedKBArticle { get; set; }
        public virtual string Explanation { get; set; }
        public virtual string Priority { get; set; }

        public virtual string Status { get; set; }

        public virtual string AssignTeam { get; set; }
        public virtual string AssignStatus { get; set; }
        public virtual string Reasoning { get; set; }
        public virtual string Plan { get; set; }
    }
}