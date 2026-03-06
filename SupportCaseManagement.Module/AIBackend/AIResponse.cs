using System.Collections.Generic;

namespace SupportCaseManagement.Module.AI
{
    public class AIResponse
    {
        public string Message { get; set; }

        public List<AIAction> Actions { get; set; } = new();
    }
}