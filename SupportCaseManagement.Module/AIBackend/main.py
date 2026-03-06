from fastapi import FastAPI
from pydantic import BaseModel
from langchain_openai import AzureChatOpenAI
from langchain_core.prompts import ChatPromptTemplate
import os
import json

app = FastAPI()

# Azure OpenAI configuration
os.environ["AZURE_OPENAI_API_KEY"] = ""
os.environ["AZURE_OPENAI_ENDPOINT"] = "https://axcces-ai.openai.azure.com"

llm = AzureChatOpenAI(
    azure_deployment="gpt-4o-mini",   # deployment name in Azure
    api_version="2024-02-15-preview",
    temperature=0
)

class CaseRequest(BaseModel):
    title: str
    description: str
    status: str
    priority: str
    comments: list[str]
    kb_articles: list[str]


prompt = ChatPromptTemplate.from_template("""
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

Case Title:
{title}

Description:
{description}

Status:
{status}

Priority:
{priority}

Comments:
{comments}

Knowledge Base Articles:
{kb}

Return JSON only.
""")

# LangChain pipeline
chain = prompt | llm


@app.post("/analyze-case")
async def analyze_case(case: CaseRequest):
    try:
        result = chain.invoke({
            "title": case.title,
            "description": case.description,
            "status": case.status,
            "priority": case.priority,
            "comments": case.comments,
            "kb": case.kb_articles
        })

        content = result.content

        # Ensure valid JSON
        parsed = json.loads(content)

        return parsed

    except Exception as e:
        return {
            "summary": "AI error",
            "plan": {
                "priority": None,
                "status": None,
                "assign_team": None,
                "kb_articles": []
            },
            "reasoning": str(e)
        }