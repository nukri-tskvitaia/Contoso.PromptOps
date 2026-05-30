# Contoso PromptOps API

## Overview

Contoso PromptOps API is a modern ASP.NET Core application built using **Clean Architecture** principles for managing and executing AI prompt templates.

The application enables teams to **create, version, activate, and execute** prompt templates against Azure OpenAI while maintaining execution history for auditing, analysis, and operational visibility.

> This project was built to explore enterprise-grade backend development practices, AI integration patterns, and modern software architecture using .NET and Azure services.

---

## Key Features

### Prompt Template Management

- Create reusable prompt templates
- Version prompt templates
- Activate and archive templates
- Store prompt metadata and configuration
- Maintain a single active version per prompt

### AI Prompt Execution

- Execute prompts against Azure OpenAI
- Combine system prompts with user input
- Capture AI-generated responses
- Track execution duration
- Store execution history for auditing

### Validation

- Request validation using **FluentValidation**
- Centralized validation rules
- Consistent validation responses

### Error Handling

- Global exception handling middleware
- Standardized Problem Details responses
- Domain-specific exception mapping
- Structured error responses

### Logging

- Request logging middleware
- Serilog integration
- Console logging
- Rolling file logs

### Health Monitoring

- Application health endpoint
- Database connectivity health checks
- Infrastructure readiness checks

### Documentation

- Swagger/OpenAPI integration
- XML documentation comments
- Endpoint documentation
- Request and response schemas

### Testing

- Unit tests using **xUnit**
- **FluentAssertions** for expressive assertions
- **NSubstitute** for mocking dependencies
- Business rule verification

---

## Architecture

The solution follows **Clean Architecture** principles.

### Architectural Goals

- Separation of concerns
- Dependency inversion
- Testability
- Maintainability
- Flexibility for changing infrastructure
- Clear ownership of business rules

### Solution Structure

```text
Contoso.PromptOps.sln
│
├── Contoso.PromptOps.Api
├── Contoso.PromptOps.Application
├── Contoso.PromptOps.Domain
├── Contoso.PromptOps.Infrastructure
└── Contoso.PromptOps.Application.UnitTests
```

---

## Domain Layer

The **Domain** layer contains the core business model and business rules.

### Responsibilities

- Business entities
- Domain validation
- Domain exceptions
- Business invariants
- Enums and value objects

### Entities

#### `PromptTemplate`

Represents a reusable AI prompt configuration.

| Property | Description |
|---|---|
| `Name` | Template name |
| `Description` | Template description |
| `SystemPrompt` | System-level prompt instructions |
| `Model` | Azure OpenAI deployment/model identifier |
| `Temperature` | Model temperature setting |
| `Version` | Template version number |
| `Status` | Activation state |

#### `PromptExecution`

Represents a single AI prompt execution.

| Property | Description |
|---|---|
| `UserInput` | User-provided input |
| `AiResponse` | Generated AI response |
| `PromptTokens` | Number of input tokens consumed, if available |
| `CompletionTokens` | Number of output tokens consumed, if available |
| `DurationMs` | Time taken to execute the prompt |
| `PromptTemplateId` | Linked prompt template reference |

---

## Application Layer

The **Application** layer contains business use cases and contracts.

### Responsibilities

- Application services
- DTOs
- Validation
- Repository abstractions
- AI provider abstractions
- Use case orchestration

### Services

#### `PromptTemplateService`

Responsible for:

- Creating prompt templates
- Updating prompt templates
- Activating templates
- Archiving templates
- Retrieving templates

#### `PromptExecutionService`

Responsible for:

- Executing prompt templates
- Calling AI providers
- Persisting execution history
- Retrieving execution history

### Abstractions

#### Repositories

- `IPromptTemplateRepository`
- `IPromptExecutionRepository`

#### AI Integration

- `IAiChatClient`

#### Persistence

- `IUnitOfWork`

---

## Infrastructure Layer

The **Infrastructure** layer implements external dependencies.

### Responsibilities

- Entity Framework Core persistence
- SQLite database integration
- Repository implementations
- Azure OpenAI integration
- Configuration handling

### Persistence

The application uses:

- **Entity Framework Core**
- **SQLite**

Implemented persistence components:

- `PromptTemplateRepository`
- `PromptExecutionRepository`
- `UnitOfWork`

### AI Provider

Current provider:

- **Azure OpenAI**

Current implementation:

- `AzureOpenAiChatClient`

> The architecture allows replacing the AI provider without modifying business logic.

Potential swappable providers:

- Azure OpenAI
- OpenAI
- Anthropic Claude
- Local LLMs

---

## API Layer

The **API** layer serves as the application's composition root.

### Responsibilities

- Dependency injection
- Controller endpoints
- Middleware registration
- Swagger configuration
- Application startup

### Controllers

#### `PromptTemplatesController`

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/prompt-templates` | Get all prompt templates |
| `GET` | `/api/prompt-templates/{id}` | Get prompt template by ID |
| `POST` | `/api/prompt-templates` | Create a prompt template |
| `PUT` | `/api/prompt-templates/{id}` | Update a prompt template |
| `POST` | `/api/prompt-templates/{id}/activate` | Activate a prompt template |
| `DELETE` | `/api/prompt-templates/{id}` | Archive a prompt template |

#### `PromptExecutionsController`

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/prompt-executions` | Execute an active prompt template |
| `GET` | `/api/prompt-executions/recent` | Get recent prompt executions |

---

## Technology Stack

| Category | Technology |
|---|---|
| Backend | .NET 10, ASP.NET Core Web API |
| Data Access | Entity Framework Core, SQLite |
| AI | Azure OpenAI, Microsoft.Extensions.AI |
| Validation | FluentValidation |
| Logging | Serilog |
| Documentation | Swagger / OpenAPI |
| Testing | xUnit, FluentAssertions, NSubstitute |

---

## Prerequisites

Before running the application, make sure the following tools and resources are available:

- .NET 10 SDK
- EF Core CLI tools
- Azure subscription
- Azure OpenAI resource
- Azure OpenAI model deployment
- Visual Studio, Rider, or VS Code

Install EF Core CLI tools if they are not already installed:

```bash
dotnet tool install --global dotnet-ef
```

```bash
dotnet ef --version
```

---

## Middleware

### Exception Handling Middleware

Provides centralized exception handling and converts exceptions into standardized **HTTP Problem Details** responses.

### Request Logging Middleware

Logs:

- HTTP method
- Request path
- Response status code
- Execution duration

---

## Configuration

### Connection String

```json
{
  "ConnectionStrings": {
    "PromptOpsDatabase": "Data Source=promptops.db"
  }
}
```

### Azure OpenAI

Store secrets using **User Secrets**:

```bash
dotnet user-secrets init

dotnet user-secrets set "AI:Endpoint" "<azure-openai-endpoint>"
dotnet user-secrets set "AI:ApiKey" "<azure-openai-api-key>"
```

Example configuration shape:

```json
{
  "AI": {
    "Provider": "AzureOpenAI",
    "Endpoint": "",
    "ApiKey": ""
  }
}
```

> Do not commit real API keys or secrets to source control.

### Temperature Limitation Note

Some Azure OpenAI models do not support custom temperature values.

For example, certain reasoning models only support the default temperature value:

```json
{
  "temperature": 1
}
```

---

## Database Setup

If migrations are already included in the repository, apply them using:

```bash
dotnet ef database update \
  --project Contoso.PromptOps.Infrastructure \
  --startup-project Contoso.PromptOps.Api
```

For first-time local setup where migrations have not been created yet, create an initial migration

```bash
dotnet ef migrations add InitialCreate \
  --project Contoso.PromptOps.Infrastructure \
  --startup-project Contoso.PromptOps.Api \
  --output-dir Persistence/Migrations
```

Then apply the migration:

```bash
dotnet ef database update \
  --project Contoso.PromptOps.Infrastructure \
  --startup-project Contoso.PromptOps.Api
```

---

## Running the Application

Start the API:

```bash
dotnet run --project Contoso.PromptOps.Api
```

### Swagger

After starting the application, navigate to:

```text
https://localhost:<port>/swagger
```

Swagger provides:

- Endpoint documentation
- Request examples
- Response schemas
- Interactive API testing

### Health Checks

Health endpoint:

```text
https://localhost:<port>/health
```

Used for:

- Application monitoring
- Readiness checks
- Infrastructure validation

---

## Example Workflow

```text
1. Create Prompt Template   →   POST /api/prompt-templates
2. Activate Template        →   POST /api/prompt-templates/{id}/activate
3. Execute Prompt           →   POST /api/prompt-executions
4. Retrieve History         →   GET  /api/prompt-executions/recent
```

---

## Example Request

### Create Prompt Template

```http
POST /api/prompt-templates
```

```json
{
  "name": "Contoso Azure Interview Coach",
  "description": "Helps engineers prepare for Azure and .NET backend interviews.",
  "systemPrompt": "You are a practical Azure and .NET interview coach. Explain clearly, focus on backend engineering, and give concise but useful answers.",
  "category": "InterviewCoach",
  "model": "o4-mini",
  "temperature": 1,
  "version": 1
}
```

### Execute Prompt

```http
POST /api/prompt-executions
```

```json
{
  "promptTemplateId": "<prompt-template-id>",
  "userInput": "Explain Azure App Service vs Azure Functions for a junior .NET developer."
}
```

---

## Testing

Run all tests:

```bash
dotnet test
```

Current test coverage includes:

- Prompt creation
- Prompt activation
- Prompt execution
- Business rule validation
- Repository interaction verification

---

## Design Decisions

### Why Clean Architecture?

Clean Architecture helps:

- Isolate business logic
- Reduce coupling
- Improve testability
- Support infrastructure changes

### Why Repository Pattern?

The Repository Pattern provides:

- Persistence abstraction
- Easier testing
- Separation between business logic and data access logic

### Why Unit of Work?

The Unit of Work pattern provides:

- Transaction boundary
- Coordinated persistence
- Explicit save behavior

### Why Azure OpenAI?

Azure OpenAI provides:

- Enterprise AI integration
- Azure ecosystem compatibility
- Production-ready deployment options
- Secure configuration and deployment patterns

### Why SQLite?

SQLite was selected for this learning project because it provides:

- Simple local development
- Real relational persistence
- EF Core support
- Easy setup without external database infrastructure

---

## Future Improvements

- [ ] Integration tests using `WebApplicationFactory`
- [ ] Authentication and authorization
- [ ] API versioning
- [ ] Docker support
- [ ] Token usage tracking
- [ ] Distributed tracing
- [ ] Metrics and observability
- [ ] Azure Entra ID integration
- [ ] CI/CD pipelines
- [ ] Role-based access control
- [ ] Prompt approval workflows
- [ ] Caching strategies
- [ ] Multi-provider AI support
- [ ] Rate limiting
- [ ] Audit dashboards

---

## Conclusion

Contoso PromptOps API demonstrates how modern AI-powered applications can be built using **Clean Architecture**, **ASP.NET Core**, **Entity Framework Core**, and **Azure OpenAI** while maintaining separation of concerns, testability, and extensibility.

For the current scope of this learning project, the implemented functionality is sufficient to demonstrate core architecture, AI integration, validation, logging, health monitoring, and testing practices. Additional enhancements can be added incrementally as requirements evolve.