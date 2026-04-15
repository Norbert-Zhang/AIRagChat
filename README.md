# 🚀 AI RAG Demo (Enterprise-Ready)

一个基于 **.NET + RAG + Agent + Function Calling** 的企业级 AI 应用示例项目。
支持知识库问答、多轮对话、工具调用（Agent）、日志追踪等能力。

---

## ✨ Features

* 🧠 **RAG（检索增强生成）**

  * 文档切分 + Embedding + 相似度搜索
* 💬 **多轮对话（Memory）**

  * 基于数据库持久化上下文
* 🤖 **Agent（工具调用）**

  * 支持多个业务工具（订单 / 用户 / 库存）
* ⚡ **Function Calling**

  * 使用 OpenAI 原生函数调用（稳定可靠）
* 📊 **PromptLog（可观测性）**

  * 记录 Prompt / Response / Token（可扩展）
* 🧩 **解耦架构**

  * 支持多模型替换（OpenAI / Azure / 本地模型）

---

## 🏗️ Architecture

```
Controller
   ↓
AiOrchestrator ⭐（核心）
   ↓
 ├── MemoryService（上下文记忆）
 ├── RagService（知识检索）
 ├── AgentTools（业务工具）
 ├── PromptLogService（日志）
 └── AIProvider（模型抽象）
        ├── OpenAIProvider（Chat）
        └── EmbeddingProvider（HTTP）
```

---

## 🛠️ Tech Stack

* .NET 8 / ASP.NET Core
* Entity Framework Core
* OpenAI API
* Semantic Kernel（用于Chat）
* Serilog（日志）

---

## 📦 Project Structure

```
/AIRagDemo
 ├── Controllers/
 ├── Services/
 │    ├── AiOrchestrator.cs
 │    ├── RagService.cs
 │    ├── OpenAIProvider.cs
 │    ├── OpenAIEmbeddingProvider.cs
 │    ├── PromptLogService.cs
 │    └── AgentTools/
 ├── Data/
 │    ├── AppDbContext.cs
 │    └── Entities/
 ├── Models/
 ├── logs/
 ├── appsettings.json
 └── Program.cs
```

---

## ⚙️ Getting Started

### 1️⃣ Clone project

```
git clone https://github.com/yourname/AI-RAG-Demo.git
cd AI-RAG-Demo
```

---

### 2️⃣ 配置 OpenAI Key

在 `appsettings.Development.json`：

```json
{
  "OpenAI": {
    "ApiKey": "your-api-key",
    "ChatModel": "gpt-4o-mini",
    "EmbeddingModel": "text-embedding-3-small"
  }
}
```

---

### 3️⃣ Run

```
dotnet run
```

---

### 4️⃣ API 测试

#### 👉 提问

```
POST /api/ai?userId=123
```

Body:

```json
{
  "message": "查询订单123状态"
}
```

---

#### 👉 导入知识库

```
POST /api/rag/ingest
```

---

## 🤖 Agent Tools 示例

| Tool         | 功能   |
| ------------ | ---- |
| GetOrder     | 查询订单 |
| GetInventory | 查询库存 |
| GetUser      | 查询用户 |

---

## 📊 Prompt Logging

系统会自动记录：

* 用户问题
* RAG上下文
* 最终Prompt
* AI响应

👉 可用于：

* Prompt调优
* 成本分析
* 错误排查

---

## 🔐 Security

* API Key 不应提交到 Git
* 使用 `.gitignore` 忽略敏感文件
* 推荐使用环境变量或 User Secrets

---

## 🚀 Roadmap

* [ ] 流式输出（Streaming）
* [ ] 前端聊天UI（类似ChatGPT）
* [ ] 向量数据库（Pinecone / pgvector）
* [ ] 多租户知识库
* [ ] 权限控制

---

## 📸 Demo（可选）

> （这里可以放截图）

---

## 🧠 What I Learned

* 如何构建完整 RAG 系统
* 如何设计 AI Orchestrator
* 如何实现 Function Calling Agent
* 如何做 AI 可观测性（PromptLog）

---

## 📄 License

MIT
