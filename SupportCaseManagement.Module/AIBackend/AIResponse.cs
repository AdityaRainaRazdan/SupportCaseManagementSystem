namespace SupportCaseManagement.Module.AIBackend
{
    public class AIResponse
    {
        public string summary { get; set; }

        public Plan plan { get; set; }

        public string reasoning { get; set; }
    }

    public class Plan
    {
        public string priority { get; set; }

        public string status { get; set; }

        public string assign_team { get; set; }

        public List<string> kb_articles { get; set; }
    }
}