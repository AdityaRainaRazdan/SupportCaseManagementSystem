// SupportCaseManagement.Module/Services/AgenticAIService.cs
using System.Net.Http;
using System.Text;
using System.Text.Json;
using DevExpress.ExpressApp;
using Microsoft.EntityFrameworkCore;
using SupportCaseManagement.Module.AIBackend;
using SupportCaseManagement.Module.BusinessObjects;
using SupportCaseManagement.Module.DTO;

namespace SupportCaseManagement.Module.Services
{
    public class AgenticAIService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "http://127.0.0.1:8000";

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AgenticAIService()
        {
            _http = new HttpClient();
        }

        // ── Build a fully-loaded snapshot of the case ─────────────────────────
        // XAF lazy-loads nav properties by default.
        // We re-query with .Include() to eagerly load:
        //   SupportCase.Comments        (ICollection<CaseComment>)
        //   SupportCase.KnowledgeLinks  (ICollection<CaseKnowledgeLink> → .Article)
        //   SupportCase.AssignedTeam    (SupportTeam)
        private static CaseSnapshot BuildSnapshot(SupportCase sc, IObjectSpace os)
        {
            var loaded = os.GetObjectsQuery<SupportCase>()
                           .Include(x => x.Comments)
                           .Include(x => x.KnowledgeLinks).ThenInclude(l => l.Article)
                           .Include(x => x.AssignedTeam)
                           .FirstOrDefault(x => x.ID == sc.ID) ?? sc;

            return new CaseSnapshot
            {
                Title = loaded.Title ?? "",
                Description = loaded.Description ?? "",
                Status = loaded.Status.ToString(),
                Priority = loaded.Priority.ToString(),
                AssignedTeam = loaded.AssignedTeam?.Name ?? "",
                // CaseComment.Text + CaseComment.CommentTypes
                Comments = loaded.Comments?
                                     .OrderBy(c => c.CreatedDate)
                                     .Select(c => $"[{c.CommentTypes}] {c.Text}")
                                     .ToList() ?? new(),
                // CaseKnowledgeLink.Article.Title
                KBArticles = loaded.KnowledgeLinks?
                                     .Where(l => l.Article != null)
                                     .Select(l => l.Article!.Title)
                                     .ToList() ?? new()
            };
        }

        // ── Load all published KB articles (KnowledgeBaseArticle.IsPublished) ──
        private static List<object> LoadAvailableKB(IObjectSpace os) =>
            os.GetObjectsQuery<KnowledgeBaseArticle>()
              .Where(k => k.IsPublished)
              .Select(k => new { id = k.ID.ToString(), title = k.Title, category = k.Category ?? "" })
              .Cast<object>()
              .ToList();

        // ── Load all team names (SupportTeam.Name) ────────────────────────────
        private static List<string> LoadTeamNames(IObjectSpace os) =>
            os.GetObjectsQuery<SupportTeam>()
              .Select(t => t.Name)
              .ToList();

        // ── Fuzzy-match AI-suggested KB titles to real DB records ─────────────
        // Checks: DB.Title contains suggestion OR suggestion contains DB.Title
        private static List<KBMatch> MatchKB(List<string>? suggestions, IObjectSpace os)
        {
            var result = new List<KBMatch>();
            if (suggestions == null) return result;
            foreach (var s in suggestions)
            {
                var found = os.GetObjectsQuery<KnowledgeBaseArticle>()
                              .Where(k => k.IsPublished &&
                                          (k.Title.Contains(s) || s.Contains(k.Title)))
                              .FirstOrDefault();
                if (found != null)
                    result.Add(new KBMatch { Id = found.ID, Title = found.Title, Category = found.Category ?? "" });
            }
            return result;
        }

        // ── Fuzzy-match AI-suggested team name to real SupportTeam row ────────
        private static (Guid? id, string? name) MatchTeam(string? suggestion, IObjectSpace os)
        {
            if (string.IsNullOrEmpty(suggestion)) return (null, null);
            var team = os.GetObjectsQuery<SupportTeam>()
                         .FirstOrDefault(t => t.Name.Contains(suggestion) || suggestion.Contains(t.Name));
            return team != null ? (team.ID, team.Name) : (null, null);
        }

        // ════════════════════════════════════════════════════════════════════════
        // PUBLIC API
        // ════════════════════════════════════════════════════════════════════════

        // ── Full analysis → AIActionProposal ──────────────────────────────────
        public async Task<AIActionProposal> AnalyzeCase(SupportCase supportCase, IObjectSpace os)
        {
            var snap = BuildSnapshot(supportCase, os);
            var allKB = LoadAvailableKB(os);
            var allTeams = LoadTeamNames(os);

            var payload = new
            {
                title = snap.Title,
                description = snap.Description,
                status = snap.Status,
                priority = snap.Priority,
                assigned_team = snap.AssignedTeam,
                comments = snap.Comments,
                kb_articles = snap.KBArticles,
                available_kb_articles = allKB,
                available_teams = allTeams
            };

            var json = await PostAsync("/analyze-case", payload);

            AIResponse ai;
            try { ai = JsonSerializer.Deserialize<AIResponse>(json, JsonOpts)!; }
            catch { throw new Exception("Invalid AI response: " + json); }

            var (teamId, teamName) = MatchTeam(ai.plan?.assign_team, os);

            return new AIActionProposal
            {
                Summary = ai.summary,
                NextSteps = ai.next_steps ?? new(),
                SuggestedPriority = ai.plan?.priority,
                SuggestedStatus = ai.plan?.status,
                AssignTeam = ai.plan?.assign_team,
                MatchedTeamId = teamId,
                MatchedTeamName = teamName,
                SuggestedKBArticle = ai.plan?.kb_articles ?? new(),
                MatchedKBArticles = MatchKB(ai.plan?.kb_articles, os),
                Reasoning = ai.reasoning,
                ProposedCustomerMessage = ai.proposed_message
            };
        }

        // ── Conversational chat about a specific case ─────────────────────────
        // Returns: text reply OR an action to execute (when user confirms)
        public async Task<ChatCaseResult> ChatWithCase(SupportCase supportCase, string message, IObjectSpace os)
        {
            var snap = BuildSnapshot(supportCase, os);
            var allKB = LoadAvailableKB(os);
            var allTeams = LoadTeamNames(os);

            var payload = new
            {
                message = message,
                title = snap.Title,
                description = snap.Description,
                status = snap.Status,
                priority = snap.Priority,
                assigned_team = snap.AssignedTeam,
                comments = snap.Comments,
                kb_articles = snap.KBArticles,
                available_kb_articles = allKB,
                available_teams = allTeams
            };

            var json = await PostAsync("/chat-case", payload);
            var parsed = JsonSerializer.Deserialize<ChatCaseResponse>(json, JsonOpts);

            if (parsed?.Action != null)
            {
                // Resolve suggested team → SupportTeam row
                var (teamId, teamName) = MatchTeam(parsed.Action.AssignTeam, os);

                // Resolve suggested KB title → KnowledgeBaseArticle row
                KBMatch? kbMatch = null;
                if (!string.IsNullOrEmpty(parsed.Action.KbArticleTitle))
                {
                    var t = parsed.Action.KbArticleTitle;
                    var kb = os.GetObjectsQuery<KnowledgeBaseArticle>()
                               .FirstOrDefault(k => k.IsPublished &&
                                                    (k.Title.Contains(t) || t.Contains(k.Title)));
                    if (kb != null)
                        kbMatch = new KBMatch { Id = kb.ID, Title = kb.Title, Category = kb.Category ?? "" };
                }

                return new ChatCaseResult
                {
                    Action = new AIActionToExecute
                    {
                        Priority = parsed.Action.Priority,
                        Status = parsed.Action.Status,
                        AssignTeam = parsed.Action.AssignTeam,
                        TeamId = teamId,
                        TeamName = teamName,
                        KbArticleTitle = parsed.Action.KbArticleTitle,
                        KbMatch = kbMatch
                    }
                };
            }

            return new ChatCaseResult { Reply = parsed?.Reply ?? json };
        }

        // ── Free conversational chat (no case selected) ───────────────────────
        public async Task<string> Chat(string message, List<ConversationMessage> history)
        {
            var payload = new
            {
                message = message,
                history = history.Select(h => new { role = h.Role, content = h.Content }).ToList()
            };
            var json = await PostAsync("/chat", payload);
            var parsed = JsonSerializer.Deserialize<ChatReply>(json, JsonOpts);
            return parsed?.Reply ?? json;
        }

        // ── Apply a plan card to the DB ───────────────────────────────────────
        // Called from Razor when user clicks "Apply These Changes" on full plan card
        public List<string> ApplyProposalToCase(AIActionProposal proposal, Guid caseId, IObjectSpace os)
        {
            var changes = new List<string>();
            var c = os.GetObjectsQuery<SupportCase>().FirstOrDefault(x => x.ID == caseId);
            if (c == null) return changes;

            // CasePriority enum: P1, P2, P3
            if (!string.IsNullOrEmpty(proposal.SuggestedPriority) &&
                Enum.TryParse<CasePriority>(proposal.SuggestedPriority.Trim(), true, out var priority))
            { c.Priority = priority; changes.Add($"Priority → {priority}"); }

            // CaseStatus enum: New, Triage, InProgress, WaitingCustomer, Resolved, Closed
            if (!string.IsNullOrEmpty(proposal.SuggestedStatus) &&
                Enum.TryParse<CaseStatus>(proposal.SuggestedStatus.Trim(), true, out var status))
            { c.Status = status; changes.Add($"Status → {status}"); }

            // SupportCase.AssignedTeam is SupportTeam (navigation property)
            if (proposal.MatchedTeamId.HasValue)
            {
                var team = os.GetObjectsQuery<SupportTeam>()
                             .FirstOrDefault(t => t.ID == proposal.MatchedTeamId.Value);
                if (team != null) { c.AssignedTeam = team; changes.Add($"Team → {team.Name}"); }
            }

            LogInteraction(os, c, "Apply AI Plan", string.Join(", ", changes));
            os.CommitChanges();
            return changes;
        }

        // ── Execute a chat-confirmed action on the DB ─────────────────────────
        public List<string> ExecuteAction(AIActionToExecute action, Guid caseId, IObjectSpace os)
        {
            var changes = new List<string>();
            var c = os.GetObjectsQuery<SupportCase>().FirstOrDefault(x => x.ID == caseId);
            if (c == null) return changes;

            if (!string.IsNullOrEmpty(action.Priority) &&
                Enum.TryParse<CasePriority>(action.Priority.Trim(), true, out var priority))
            { c.Priority = priority; changes.Add($"Priority → {priority}"); }

            if (!string.IsNullOrEmpty(action.Status) &&
                Enum.TryParse<CaseStatus>(action.Status.Trim(), true, out var status))
            { c.Status = status; changes.Add($"Status → {status}"); }

            // SupportCase.AssignedTeam = SupportTeam
            if (action.TeamId.HasValue)
            {
                var team = os.GetObjectsQuery<SupportTeam>()
                             .FirstOrDefault(t => t.ID == action.TeamId.Value);
                if (team != null) { c.AssignedTeam = team; changes.Add($"Team → {team.Name}"); }
            }

            // CaseKnowledgeLink: SupportCaseId + KnowledgeBaseArticleId
            if (action.KbMatch != null)
            {
                var alreadyLinked = os.GetObjectsQuery<CaseKnowledgeLink>()
                                      .Any(l => l.SupportCaseId == caseId &&
                                                l.KnowledgeBaseArticleId == action.KbMatch.Id);
                if (!alreadyLinked)
                {
                    var link = os.CreateObject<CaseKnowledgeLink>();
                    link.SupportCaseId = caseId;
                    link.KnowledgeBaseArticleId = action.KbMatch.Id;
                    changes.Add($"Linked KB: {action.KbMatch.Title}");
                }
                else changes.Add($"KB already linked: {action.KbMatch.Title}");
            }

            LogInteraction(os, c, "Chat Action Confirmed", string.Join(", ", changes));
            os.CommitChanges();
            return changes;
        }

        // ── Link a single KB article to a case ────────────────────────────────
        // Called from plan card "🔗 Link" button
        public void LinkKBArticle(KBMatch kb, Guid caseId, IObjectSpace os)
        {
            var alreadyLinked = os.GetObjectsQuery<CaseKnowledgeLink>()
                                  .Any(l => l.SupportCaseId == caseId &&
                                            l.KnowledgeBaseArticleId == kb.Id);
            if (!alreadyLinked)
            {
                var link = os.CreateObject<CaseKnowledgeLink>();
                link.SupportCaseId = caseId;
                link.KnowledgeBaseArticleId = kb.Id;
                os.CommitChanges();
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private static void LogInteraction(IObjectSpace os, SupportCase c, string userMsg, string aiResponse)
        {
            var log = os.CreateObject<AIInteractionLog>();
            log.Case = c;
            log.UserMessage = userMsg;
            log.AIResponse = aiResponse;
            log.AIModel = "gpt-4o-mini";
            log.PlanApplied = true;
            log.Timestamp = DateTime.UtcNow;
        }

        private async Task<string> PostAsync(string path, object body)
        {
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(BaseUrl + path, content);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"AI service error ({response.StatusCode}): {result}");
            return result;
        }

        // private DTOs for deserialization only
        private class ChatReply { public string Reply { get; set; } = ""; }
        private class ChatCaseResponse
        {
            public string? Reply { get; set; }
            public ChatActionPayload? Action { get; set; }
        }
        private class ChatActionPayload
        {
            public string? Action { get; set; }
            public string? Priority { get; set; }
            public string? Status { get; set; }
            public string? AssignTeam { get; set; }
            public string? KbArticleTitle { get; set; }
        }
    }

    // ── Shared transfer objects used by Razor ─────────────────────────────────

    public class CaseSnapshot
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Status { get; set; } = "";
        public string Priority { get; set; } = "";
        public string AssignedTeam { get; set; } = "";
        public List<string> Comments { get; set; } = new();
        public List<string> KBArticles { get; set; } = new();
    }

    public class ChatCaseResult
    {
        public string? Reply { get; set; }
        public AIActionToExecute? Action { get; set; }
    }

    public class AIActionToExecute
    {
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public string? AssignTeam { get; set; }   // raw string from AI
        public Guid? TeamId { get; set; }   // resolved SupportTeam.ID
        public string? TeamName { get; set; }   // resolved SupportTeam.Name
        public string? KbArticleTitle { get; set; }   // raw title from AI
        public KBMatch? KbMatch { get; set; }   // resolved KnowledgeBaseArticle
    }

    public class ConversationMessage
    {
        public string Role { get; set; } = "";
        public string Content { get; set; } = "";
    }
}