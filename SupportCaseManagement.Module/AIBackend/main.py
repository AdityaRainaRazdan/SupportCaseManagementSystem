# ai_backend/main.py
from fastapi import FastAPI
from pydantic import BaseModel
from langchain_openai import AzureChatOpenAI
from langchain_core.prompts import ChatPromptTemplate
from langchain_core.messages import SystemMessage, HumanMessage, AIMessage
import os, json

app = FastAPI()

os.environ["AZURE_OPENAI_API_KEY"] = ""
os.environ["AZURE_OPENAI_ENDPOINT"] = "https://axcces-ai.openai.azure.com"

llm = AzureChatOpenAI(
    azure_deployment="gpt-4o-mini",
    api_version="2024-02-15-preview",
    temperature=0
)

# ── Pydantic models ────────────────────────────────────────────────────────────

class CaseRequest(BaseModel):
    title: str
    description: str
    status: str                         # CaseStatus enum string
    priority: str                       # CasePriority enum string
    assigned_team: str = ""             # SupportTeam.Name currently assigned
    comments: list[str] = []            # "[CommentType] text" formatted strings
    kb_articles: list[str] = []         # titles of already-linked KB articles
    available_kb_articles: list[dict] = []  # [{id, title, category}] from DB
    available_teams: list[str] = []     # [SupportTeam.Name, ...] from DB

class ChatRequest(BaseModel):
    message: str
    title: str = ""
    description: str = ""
    status: str = ""
    priority: str = ""
    assigned_team: str = ""
    comments: list[str] = []
    kb_articles: list[str] = []
    available_kb_articles: list[dict] = []
    available_teams: list[str] = []

class HistoryMessage(BaseModel):
    role: str       # "user" | "assistant" | "system_context"
    content: str

class FreeMessage(BaseModel):
    message: str
    history: list[HistoryMessage] = []

# ── Prompts ────────────────────────────────────────────────────────────────────

ANALYZE_PROMPT = ChatPromptTemplate.from_template("""
You are an expert AI support assistant. Analyze the support case thoroughly.

VALID STATUS VALUES (use exactly one): New, Triage, InProgress, WaitingCustomer, Resolved, Closed
VALID PRIORITY VALUES (use exactly one): P1, P2, P3
AVAILABLE TEAMS (use exact name or null): {available_teams}
AVAILABLE KB ARTICLES (use exact title or null): {available_kb}

Return ONLY valid JSON — no markdown fences, no extra text:
{{
  "summary": "2-3 sentence summary of the issue, current state, and business impact",
  "next_steps": [
    "First concrete action for the agent",
    "Second action",
    "Third action if applicable"
  ],
  "plan": {{
    "priority": "P1 or P2 or P3",
    "status": "exact status from valid list",
    "assign_team": "exact team name from available teams, or null",
    "kb_articles": ["exact title from available KB articles that are relevant. Empty array if none match."]
  }},
  "reasoning": "Specific reasoning referencing case details: why this priority, status, team, and KB articles",
  "proposed_message": "Professional customer-facing reply message ready to send"
}}

Case Title: {title}
Description: {description}
Current Status: {status}
Current Priority: {priority}
Currently Assigned Team: {assigned_team}
Comments:
{comments}
KB Articles Already Linked:
{kb}

Return JSON only.
""")

CHAT_PROMPT = ChatPromptTemplate.from_template("""
You are an AI support assistant helping an agent work on this specific support case.

VALID STATUS VALUES: New, Triage, InProgress, WaitingCustomer, Resolved, Closed
VALID PRIORITY VALUES: P1, P2, P3
AVAILABLE SUPPORT TEAMS (only use these exact names): {available_teams}
AVAILABLE KB ARTICLES (only use these exact titles): {available_kb}

CASE DETAILS:
Title: {title}
Description: {description}
Current Status: {status}
Current Priority: {priority}
Currently Assigned Team: {assigned_team}
Comments:
{comments}
KB Articles Linked:
{kb}

BEHAVIOUR RULES:
1. For informational requests (summarize, next steps, explain, draft):
   - Answer directly and helpfully.
   - When summarizing, include ALL comments listed above in your response.
   - When recommending KB articles, only suggest titles from AVAILABLE KB ARTICLES.
   - When recommending a team, only use names from AVAILABLE SUPPORT TEAMS.

2. For change requests (set priority, change status, assign team, link KB article):
   - Propose conversationally and ask for confirmation.
   - Example: "I suggest setting priority to P2 and assigning to [Team] because [reason]. Shall I apply this?"

3. When user CONFIRMS (says yes / ok / sure / confirm / apply / do it / go ahead):
   - Return ONLY this JSON object — no surrounding text whatsoever:
   {{"action":"apply_changes","priority":"P1|P2|P3 or null","status":"exact status or null","assign_team":"exact team name or null","kb_article_title":"exact KB title or null"}}

4. Never invent team names or KB titles not in the provided lists.

User message: {message}
""")

analyze_chain = ANALYZE_PROMPT | llm
chat_chain    = CHAT_PROMPT    | llm

# ── Helpers ────────────────────────────────────────────────────────────────────

def fmt_list(items: list[str], empty_msg: str = "None.") -> str:
    return "\n".join(f"  - {i}" for i in items) if items else f"  {empty_msg}"

def strip_fences(text: str) -> str:
    text = text.strip()
    if text.startswith("```"):
        parts = text.split("```")
        text = parts[1] if len(parts) > 1 else text
        if text.startswith("json"):
            text = text[4:]
    return text.strip()

# ── Endpoints ──────────────────────────────────────────────────────────────────

@app.post("/analyze-case")
async def analyze_case(req: CaseRequest):
    try:
        result = analyze_chain.invoke({
            "title":           req.title,
            "description":     req.description or "No description provided.",
            "status":          req.status,
            "priority":        req.priority,
            "assigned_team":   req.assigned_team or "Not assigned",
            "comments":        fmt_list(req.comments, "No comments yet."),
            "kb":              fmt_list(req.kb_articles, "None linked."),
            "available_kb":    json.dumps(req.available_kb_articles, indent=2),
            "available_teams": json.dumps(req.available_teams)
        })
        return json.loads(strip_fences(result.content))
    except Exception as e:
        return {
            "summary": "Analysis failed.",
            "next_steps": [],
            "plan": {"priority": None, "status": None, "assign_team": None, "kb_articles": []},
            "reasoning": str(e),
            "proposed_message": None
        }

@app.post("/chat-case")
async def chat_case(req: ChatRequest):
    try:
        result = chat_chain.invoke({
            "message":         req.message,
            "title":           req.title,
            "description":     req.description or "No description provided.",
            "status":          req.status,
            "priority":        req.priority,
            "assigned_team":   req.assigned_team or "Not assigned",
            "comments":        fmt_list(req.comments, "No comments yet."),
            "kb":              fmt_list(req.kb_articles, "None linked."),
            "available_kb":    json.dumps(req.available_kb_articles, indent=2),
            "available_teams": json.dumps(req.available_teams)
        })
        raw = result.content.strip()

        # Detect action JSON — AI returns bare JSON when user confirms
        if raw.startswith("{") and '"action"' in raw:
            try:
                action = json.loads(raw)
                return {"reply": None, "action": action}
            except json.JSONDecodeError:
                pass

        return {"reply": raw, "action": None}

    except Exception as e:
        return {"reply": f"AI error: {str(e)}", "action": None}

@app.post("/chat")
async def chat(req: FreeMessage):
    try:
        messages = [SystemMessage(content="""You are a helpful AI support assistant for a case management system.
Help agents analyze cases, suggest priorities, recommend KB articles, and answer support questions.
When given case data, use it to give specific, accurate answers.
When suggesting changes, always phrase as a proposal the user must confirm.
Be conversational, concise, and helpful.""")]

        for h in req.history:
            if   h.role == "system_context": messages.append(SystemMessage(content=h.content))
            elif h.role == "user":           messages.append(HumanMessage(content=h.content))
            elif h.role == "assistant":      messages.append(AIMessage(content=h.content))

        messages.append(HumanMessage(content=req.message))
        result = llm.invoke(messages)
        return {"reply": result.content}

    except Exception as e:
        return {"reply": f"AI error: {str(e)}"}