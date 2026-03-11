from fastapi import FastAPI
from pydantic import BaseModel
from langchain_openai import AzureChatOpenAI
from langchain_core.prompts import ChatPromptTemplate
from langchain_core.messages import SystemMessage, HumanMessage, AIMessage
import os
import json

app = FastAPI()

os.environ["AZURE_OPENAI_ENDPOINT"] = "https://axcces-ai.openai.azure.com"

llm = AzureChatOpenAI(
    azure_deployment="gpt-4o-mini",
    api_version="2024-02-15-preview",
    temperature=0
)

# ── Models ─────────────────────────────────────────────────────────────────────

class CaseRequest(BaseModel):
    title: str
    description: str
    status: str
    priority: str
    comments: list[str]
    kb_articles: list[str]

class ChatRequest(BaseModel):
    message: str
    title: str = ""
    description: str = ""
    status: str = ""
    priority: str = ""
    comments: list[str] = []
    kb_articles: list[str] = []

class HistoryMessage(BaseModel):
    role: str       # "user", "assistant", "system_context"
    content: str

class FreeMessage(BaseModel):
    message: str
    history: list[HistoryMessage] = []

# ── Prompts ────────────────────────────────────────────────────────────────────

analyze_prompt = ChatPromptTemplate.from_template("""
You are an AI support assistant.
Analyze the support case and suggest improvements.
Return ONLY JSON in the following format:
{{
  "summary": "Short summary of the issue",
  "plan": {{
    "priority": "P1/P2/P3 or null",
    "status": "New/InProgress/Resolved or null",
    "assign_team": "Team name or null",
    "kb_articles": []
  }},
  "reasoning": "Explanation of why the suggestion was made"
}}
Case Title: {title}
Description: {description}
Status: {status}
Priority: {priority}
Comments: {comments}
Knowledge Base Articles: {kb}
Return JSON only.
""")

chat_case_prompt = ChatPromptTemplate.from_template("""
You are a helpful AI support assistant reviewing a specific support case.
Answer conversationally and helpfully. Do NOT return JSON.

Case Title: {title}
Description: {description}
Status: {status}
Priority: {priority}
Comments: {comments}
Knowledge Base Articles: {kb}

User: {message}
""")

analyze_chain = analyze_prompt | llm
chat_case_chain = chat_case_prompt | llm

# ── Endpoints ──────────────────────────────────────────────────────────────────

@app.post("/analyze-case")
async def analyze_case(case: CaseRequest):
    try:
        result = analyze_chain.invoke({
            "title": case.title,
            "description": case.description,
            "status": case.status,
            "priority": case.priority,
            "comments": case.comments,
            "kb": case.kb_articles
        })
        return json.loads(result.content)
    except Exception as e:
        return {
            "summary": "AI error",
            "plan": {"priority": None, "status": None, "assign_team": None, "kb_articles": []},
            "reasoning": str(e)
        }

@app.post("/chat-case")
async def chat_case(req: ChatRequest):
    try:
        result = chat_case_chain.invoke({
            "message": req.message,
            "title": req.title,
            "description": req.description,
            "status": req.status,
            "priority": req.priority,
            "comments": req.comments,
            "kb": req.kb_articles
        })
        return {"reply": result.content}
    except Exception as e:
        return {"reply": f"AI error: {str(e)}"}

@app.post("/chat")
async def chat(req: FreeMessage):
    try:
        # ✅ Build message list — system context + history + current message
        messages = [
            SystemMessage(content="""You are a helpful AI support assistant for a case management system.
You help agents analyze cases, suggest priorities, recommend knowledge base articles, and answer support questions.
When given case data, use it to give specific, accurate answers about those cases.
Be conversational, concise, and helpful.""")
        ]

        for h in req.history:
            if h.role == "system_context":
                # Inject case data as additional system context
                messages.append(SystemMessage(content=h.content))
            elif h.role == "user":
                messages.append(HumanMessage(content=h.content))
            elif h.role == "assistant":
                messages.append(AIMessage(content=h.content))

        messages.append(HumanMessage(content=req.message))

        result = llm.invoke(messages)
        return {"reply": result.content}

    except Exception as e:
        return {"reply": f"AI error: {str(e)}"}