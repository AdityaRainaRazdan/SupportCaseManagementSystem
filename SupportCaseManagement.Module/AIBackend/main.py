from fastapi import FastAPI
from pydantic import BaseModel
from langchain_openai import ChatOpenAI
from langchain_core.prompts import ChatPromptTemplate
import os
import json

app = FastAPI()

# Set API Key
os.environ["OPENAI_API_KEY"] = ""

llm = ChatOpenAI(model="gpt-4o-mini")

# -------------------------
# Request Model
# -------------------------

class CaseRequest(BaseModel):
    title: str
    description: str
    status: str
    priority: str
    comments: list[str]
    kb_articles: list[str]


# -------------------------
# Prompt Template
# -------------------------

prompt = ChatPromptTemplate.from_template(
"""
You are an AI assistant helping support agents manage cases.

CASE
Title: {title}
Description: {description}
Status: {status}
Priority: {priority}

COMMENTS
{comments}

KNOWLEDGE BASE
{kb}

Analyze the case and return JSON:

{
"summary": "...",
"plan": {
    "priority": "P1 | P2 | P3 | null",
    "status": "New | Triage | InProgress | WaitingCustomer | Resolved | Closed | null",
    "assign_team": "...",
    "kb_articles": ["..."]
},
"reasoning": "Why these actions are recommended"
}
"""
)

# -------------------------
# Endpoint
# -------------------------

@app.post("/analyze-case")
async def analyze_case(data: CaseRequest):

    formatted_prompt = prompt.format(
        title=data.title,
        description=data.description,
        status=data.status,
        priority=data.priority,
        comments="\n".join(data.comments),
        kb="\n".join(data.kb_articles)
    )

    result = llm.invoke(formatted_prompt)

    try:
        parsed = json.loads(result.content)
    except:
        parsed = {"summary": result.content}

    return parsed