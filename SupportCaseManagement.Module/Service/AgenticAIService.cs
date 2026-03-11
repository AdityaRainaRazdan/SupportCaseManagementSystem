using System.Net.Http;
using System.Text;
using System.Text.Json;
using SupportCaseManagement.Module.AIBackend;
using SupportCaseManagement.Module.BusinessObjects;
using SupportCaseManagement.Module.DTO;

namespace SupportCaseManagement.Module.Services
{
    public class AgenticAIService
    {
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AgenticAIService()
        {
            _httpClient = new HttpClient();
        }

        // ── Analyze a case → returns structured plan ──────────────────────────
        public async Task<AIActionProposal> AnalyzeCase(SupportCase supportCase)
        {
            var request = new
            {
                title = supportCase.Title,
                description = supportCase.Description,
                status = supportCase.Status.ToString(),
                priority = supportCase.Priority.ToString(),
                comments = supportCase.Comments?.Select(c => c.Text).ToList() ?? new List<string>(),
                kb_articles = supportCase.KnowledgeLinks?.Select(k => k.Article?.Title).ToList() ?? new List<string>()
            };

            var response = await _httpClient.PostAsync(
                "http://127.0.0.1:8000/analyze-case",
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            );

            var json = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception("AI service error: " + json);

            AIResponse aiResponse;
            try { aiResponse = JsonSerializer.Deserialize<AIResponse>(json, JsonOptions); }
            catch { throw new Exception("Invalid AI JSON response: " + json); }

            return new AIActionProposal
            {
                Summary = aiResponse.summary,
                SuggestedPriority = aiResponse.plan?.priority,
                SuggestedStatus = aiResponse.plan?.status,
                AssignTeam = aiResponse.plan?.assign_team,
                SuggestedKBArticle = aiResponse.plan?.kb_articles ?? new List<string>(),
                Reasoning = aiResponse.reasoning
            };
        }

        // ── Chat about a specific case ────────────────────────────────────────
        public async Task<string> ChatWithCase(SupportCase supportCase, string message)
        {
            var request = new
            {
                message = message,
                title = supportCase.Title,
                description = supportCase.Description,
                status = supportCase.Status.ToString(),
                priority = supportCase.Priority.ToString(),
                comments = supportCase.Comments?.Select(c => c.Text).ToList() ?? new List<string>(),
                kb_articles = supportCase.KnowledgeLinks?.Select(k => k.Article?.Title).ToList() ?? new List<string>()
            };

            var response = await _httpClient.PostAsync(
                "http://127.0.0.1:8000/chat-case",
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            );

            var json = await response.Content.ReadAsStringAsync();
            try
            {
                var parsed = JsonSerializer.Deserialize<ChatReply>(json, JsonOptions);
                return parsed?.Reply ?? "No reply from AI.";
            }
            catch { return json; }
        }

        // ✅ Free conversational chat — no case required, supports history
        public async Task<string> Chat(string message, List<ConversationMessage> history)
        {
            var request = new
            {
                message = message,
                history = history.Select(h => new { role = h.Role, content = h.Content }).ToList()
            };

            var response = await _httpClient.PostAsync(
                "http://127.0.0.1:8000/chat",
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            );

            var json = await response.Content.ReadAsStringAsync();
            try
            {
                var parsed = JsonSerializer.Deserialize<ChatReply>(json, JsonOptions);
                return parsed?.Reply ?? "No reply from AI.";
            }
            catch { return json; }
        }

        private class ChatReply
        {
            public string Reply { get; set; }
        }
    }

    // Represents one message in conversation history
    public class ConversationMessage
    {
        public string Role { get; set; }    // "user" or "assistant"
        public string Content { get; set; }
    }
}