using System.Net.Http;
using System.Text;
using System.Text.Json;
using SupportCaseManagement.Module.DTO;
using SupportCaseManagement.Module.BusinessObjects;
using SupportCaseManagement.Module.AI;
using SupportCaseManagement.Module.AIBackend;
using DevExpress.Data.Exceptions;

namespace SupportCaseManagement.Module.Services
{
    public class AgenticAIService
    {
        private readonly HttpClient httpClient;

        public AgenticAIService()
        {
            httpClient = new HttpClient();
        }

        public async Task<AIActionProposal> AnalyzeCase(SupportCase supportCase)
        {
            var httpClient = new HttpClient();

            var request = new
            {
                title = supportCase.Title,
                description = supportCase.Description,
                status = supportCase.Status.ToString(),
                priority = supportCase.Priority.ToString(),
                comments = supportCase.Comments?.Select(c => c.Text).ToList(),
                kb_articles = supportCase.KnowledgeLinks?.Select(k => k.Article?.Title).ToList()
            };

            var jsonRequest = JsonSerializer.Serialize(request);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(
                "http://127.0.0.1:8000/analyze-case",
                content
            );

            var responseJson = await response.Content.ReadAsStringAsync();

            Console.WriteLine("AI RESPONSE:");
            Console.WriteLine(responseJson);

            if (!response.IsSuccessStatusCode)
                throw new Exception("AI service error: " + responseJson);

            AIResponse aiResponse;

            try
            {
                aiResponse = JsonSerializer.Deserialize<AIResponse>(
                    responseJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch
            {
                throw new Exception("Invalid AI JSON response: " + responseJson);
            }

            // 🔹 Convert AIResponse → AIActionProposal
            var proposal = new AIActionProposal
            {
                Summary = aiResponse.summary,
                SuggestedPriority = aiResponse.plan?.priority,
                SuggestedStatus = aiResponse.plan?.status,
                AssignTeam = aiResponse.plan?.assign_team,
                SuggestedKBArticle = aiResponse.plan?.kb_articles ?? new List<string>(),
                Reasoning = aiResponse.reasoning
            };

            return proposal;
        }

        public async Task<string> ChatWithCase(SupportCase supportCase, string message)
        {
            message = message.ToLower();

if (message.Contains("analyze"))
            {
                var proposal = await AnalyzeCase(supportCase);

                return $@"

AI ANALYSIS

{proposal.Summary}

Suggested Plan
Priority: {proposal.SuggestedPriority}
Status: {proposal.SuggestedStatus}
Team: {proposal.AssignTeam}

Reply with 'apply plan' if you want me to execute it.";
            }


if (message.Contains("apply"))
            {
                supportCase.Priority = CasePriority.P1;
                supportCase.Status = CaseStatus.InProgress;

                return "Plan executed successfully.";
            }

            return "You can ask me to 'analyze the case' or 'apply plan'.";


}

    }
}