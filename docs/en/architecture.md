# DevJourney Architecture

## 1. Introduction

DevJourney is a personal platform for presenting professional identity, skills, career journey, educational courses, projects, and technical articles. In addition to the public-facing website, the system includes an administration area for managing content, settings, comments, analytics, and other operational concerns.

The goal of the DevJourney architecture is to provide a system that remains simple and appropriate for its actual size while still being maintainable, testable, extensible, and technically well-structured.

Architectural complexity is not considered a goal by itself. A technology, pattern, abstraction, or infrastructure component is introduced only when it solves a real problem and provides enough value to justify its additional complexity.

---

## 2. Selected Architecture

DevJourney uses a **Layered Monolithic Architecture** following the core principles of **Clean Architecture**.

The main layers are:

```text id="9k4d8c"
Presentation
     ↓
Application
     ↓
Domain

Infrastructure
     ↓
Application
     ↓
Domain
```

The solution is organized around four primary layers:

```text id="x2c8m4"
Presentation
Application
Domain
Infrastructure
```

The system is intentionally implemented as a Monolith. Considering the current scope and deployment model, Microservices or a more complex Modular Monolith would introduce additional architectural and operational overhead without solving a corresponding problem.

---

## 3. Architectural Goals

The architecture is designed around the following goals:

- Separation of Concerns
- Dependency Inversion
- Maintainability
- Testability
- Extensibility
- Simplicity
- Explicit Business Logic
- Controlled Infrastructure Dependencies
- The ability to evolve without being tightly coupled to a specific provider or technology

The architecture should make the system easier to understand and modify rather than simply increasing the number of layers, patterns, or abstractions.

---

## 4. Layer Responsibilities

### 4.1 Presentation

Presentation is the entry point for users and the interaction layer of the application.

It is implemented using **ASP.NET Core Razor Pages**.

Primary responsibilities include:

- Rendering public pages
- Rendering the administration area
- Handling user input
- Model Binding
- Displaying validation errors
- Authentication and authorization at the presentation boundary
- Routing
- UI localization
- Presenting View Models
- Communicating with the Application Layer

Presentation must not contain core business logic.

For example:

```text id="s0j2qk"
Razor Page
    ↓
IArticleService
    ↓
Application
```

rather than directly accessing `DbContext`, Redis, AI providers, or other infrastructure implementations.

---

### 4.2 Application

The Application Layer contains application use cases and orchestrates interactions between the Domain and Infrastructure abstractions.

The layer follows a Type-Based organization:

```text id="f7w4p3"
Application/
├── Services/
├── Interfaces/
├── DTOs/
├── Validators/
├── Mappers/
├── Exceptions/
├── Common/
└── DependencyInjection/
```

Classic Application Services are used for the main business operations.

Examples:

```text id="3t8l5e"
ArticleService
CommentService
ProjectService
CourseService
AnalyticsService
SettingsService
```

Application code may depend on abstractions such as repositories, AI services, cache services, or external service interfaces without depending on their Infrastructure implementations.

---

### 4.3 Domain

The Domain Layer represents the core business model of DevJourney and remains independent of infrastructure technologies.

Its responsibilities include:

- Entities
- Value Objects
- Enums
- Business Rules
- Domain Services
- Factories
- Domain Events
- Domain Exceptions

The Domain must not directly depend on:

```text id="w9p6z4"
EF Core
Dapper
Redis
Hangfire
AI SDKs
Razor Pages
ASP.NET Core
```

Business rules should remain inside the Domain whenever possible, while the Application Layer is responsible for orchestrating the execution of those rules.

---

### 4.4 Infrastructure

Infrastructure contains technical implementations and integrations with external systems.

Typical structure:

```text id="5c1q7r"
Infrastructure/
├── Persistence/
├── Caching/
├── Localization/
├── AI/
├── BackgroundJobs/
├── ExternalServices/
├── Security/
├── Logging/
├── Configuration/
└── DependencyInjection/
```

Infrastructure provides implementations for abstractions defined by Application and contains technology-specific concerns such as databases, caching, AI providers, background processing, and external APIs.

---

## 5. Dependency Rules

The primary dependency rule is that the Domain must not depend on outer layers.

```text id="v4r8s1"
Presentation
      ↓
Application
      ↓
Domain
```

Infrastructure depends on Application abstractions:

```text id="j2f9xk"
Infrastructure
      ↓
Application
      ↓
Domain
```

Application must therefore not directly reference Infrastructure implementations.

For example:

```text id="6yq1n8"
Application
    ↓
IArticleRepository
```

while:

```text id="7p4m3c"
Infrastructure
    ↓
ArticleRepository : IArticleRepository
```

This keeps business logic independent from persistence and other infrastructure details.

---

## 6. Type-Based Organization

DevJourney uses **Type-Based Organization** throughout the solution.

For example:

```text id="8f2q0m"
Application/
├── Services/
├── DTOs/
├── Validators/
└── Interfaces/
```

rather than creating a separate set of handlers and supporting classes for every individual use case.

This decision is based on the size and expected complexity of the project. The number of business capabilities is limited, and classic services provide a simpler and more discoverable structure.

---

## 7. Application Services

DevJourney uses classic Application Services rather than a Mediator-based request pipeline.

For example:

```text id="3p8w1a"
Presentation
      ↓
IArticleService
      ↓
ArticleService
      ↓
Domain / Persistence
```

Services are organized around clear business responsibilities and should not evolve into God Services.

Typical examples include:

```text id="9b2m7f"
ArticleService
CommentService
AnalyticsService
SettingsService
```

A service is kept intact while its responsibilities remain cohesive and reasonably sized. It is not split into numerous handlers merely for structural granularity.

---

## 8. Design Patterns

Design patterns in DevJourney are not introduced for demonstration purposes or simply to increase the number of patterns in the codebase.

A pattern is introduced only when it solves a real recurring problem better than a simpler alternative.

The decision process is:

```text id="r8c2n5"
Problem
   ↓
Simple Solution
   ↓
Does a Pattern Provide Real Value?
   ↓
Yes → Use the Pattern
No  → Keep the Simple Solution
```

### Factory Method

Factory Method is used when creating an entity involves meaningful validation, invariants, or creation rules.

Examples may include:

```text id="m3p8w7"
ArticleFactory
CommentFactory
```

Factories are not created for simple entities where direct construction is sufficient.

### Repository

Repositories are used where a meaningful persistence abstraction exists for a domain concept.

Repositories are specific rather than generic.

Examples:

```text id="k5f9x3"
IArticleRepository
ICommentRepository
IProjectRepository
```

A Generic Repository abstraction is intentionally avoided.

### Unit of Work

EF Core's `DbContext` already provides Unit of Work behavior.

If the application requires an explicit abstraction around the transaction boundary, a thin `IUnitOfWork` may be defined in Application and implemented by Infrastructure.

A generic or highly abstracted Unit of Work is intentionally avoided.

### Strategy

Strategy is appropriate when multiple real implementations of the same behavior may exist.

A relevant example is interchangeable AI or external service providers:

```text id="w4c7s2"
IAiModerationService
        │
        ├── Provider A
        └── Provider B
```

### Adapter

Adapters isolate the application from external APIs and SDKs.

Typical examples include:

```text id="n8r2q6"
GitHub API
AI Provider
Email Provider
```

Application interacts with abstractions rather than external SDKs directly.

### Decorator

Decorator may be used for cross-cutting concerns such as caching when separation from the underlying service provides meaningful value.

For example:

```text id="c6p9m1"
IArticleService
      ↑
CachedArticleService
      ↑
ArticleService
```

It is not introduced unless this separation is actually beneficial.

### State

Articles have a lifecycle such as:

```text id="x3n7q5"
Draft
Scheduled
Published
Archived
```

If state-dependent behavior becomes sufficiently complex, the State Pattern may be introduced. If the lifecycle remains simple, an enum and conventional business rules are preferred.

---

## 9. Architectural Decisions & Trade-offs

### Layered Monolith instead of Microservices

DevJourney is a personal website with a centralized application and a relatively small domain.

Splitting the system into multiple Microservices would introduce additional concerns such as network communication, distributed failure, independent deployment, and cross-service consistency without providing enough value for the current problem.

A Layered Monolith is therefore the more appropriate choice.

### No Message Broker

RabbitMQ and other Message Brokers are intentionally excluded from the current architecture.

Asynchronous and scheduled processing can be handled through application-level background processing and scheduled jobs.

For the current scope, introducing a Message Broker would add more complexity than practical value.

### No Mediator

MediatR and a Mediator-based application pipeline are intentionally not used.

Classic Application Services are sufficient for the current number and complexity of use cases and avoid an additional layer of indirection.

### No Mandatory CQRS

The application does not enforce a full Command/Query Separation model for every operation.

When a query becomes sufficiently complex, a dedicated read implementation using Dapper may be introduced without turning the entire application into a CQRS architecture.

### No Generic Repository

A single generic repository abstraction is not used for all entities.

Repositories, when required, are designed around the actual persistence requirements of the corresponding domain concepts.

### PostgreSQL

PostgreSQL is used as the primary relational database.

Reasons include:

- Strong relational capabilities
- Powerful SQL
- Transaction support
- Indexing capabilities
- JSON support when appropriate
- Suitability for the project's transactional and analytical workloads
- Strong support within the .NET ecosystem

### EF Core

EF Core is the primary persistence technology and is mainly used for transactional writes and normal persistence operations.

Reasons include:

- Excellent integration with ASP.NET Core
- Change tracking
- Transaction support
- Migrations
- Entity mapping
- Reduced need for handwritten SQL for conventional operations

### Dapper

Dapper is not used as the primary persistence technology.

It is introduced selectively for queries that benefit from explicit SQL, specialized projections, reporting, or analytical queries.

This keeps normal persistence simple with EF Core while providing SQL-level control when necessary.

### Redis and HybridCache

Caching is used for frequently accessed and relatively stable data such as:

```text id="b7x2k1"
Profile
Projects
Courses
Published Articles
Dashboard Metrics
```

HybridCache provides a unified approach for local memory caching and distributed caching, while Redis provides distributed cache storage.

Caching remains selective rather than being applied indiscriminately across the entire application.

### Hangfire

Hangfire is used for background jobs that require reliable scheduling and persistence.

A representative use case is:

```text id="m4w8q0"
Publish Scheduled Article
```

Simpler background processing may use `BackgroundService` or other native background mechanisms.

### AI as Infrastructure Integration

AI functionality is not part of the Domain model.

The Application Layer interacts with abstractions such as:

```text id="f9k2m7"
IAiModerationService
IArticleWritingService
```

while the actual provider integration resides in Infrastructure.

This keeps the application independent from a specific AI provider and allows the provider to be replaced when necessary.

---

## 10. Multilingual Architecture

Multilingual support is treated as a general system capability, while the way content is modeled for multiple languages is determined by the Domain requirements of each entity.

### UI Localization

Static user interface text is handled through ASP.NET Core Localization and resource files.

```text id="x5m2r8"
fa
en
```

### Content Localization

Entities such as Profile, Project, Course, and Timeline represent a single conceptual entity with multiple localized representations.

For example:

```text id="q8k1v3"
Project
├── Common Data
└── ProjectTranslation
    ├── fa
    └── en
```

### Article Language

Articles are independent content items with their own language.

For example:

```text id="w2k7p4"
Article
├── Language = fa
```

or:

```text id="q4x8m1"
Article
├── Language = en
```

A Persian article and an English article may therefore be two independent records rather than translations of the same entity.

This reflects the actual content model of the blog.

---

## 11. Language Detection

The administrator can configure the site's language policy:

```text id="p6m8q2"
Persian
English
Automatic
```

When Automatic mode is selected, the preferred language is determined using the following order:

```text id="z1c7r4"
Explicit User Preference
        ↓
URL
        ↓
Cookie
        ↓
Browser Accept-Language
        ↓
IP / Geo
        ↓
Time Zone
        ↓
Default Language
```

An explicit user preference always has higher priority than automatic detection.

Users can also manually switch to the alternative language regardless of the automatically detected language.

---

## 12. Settings

Runtime application settings are managed through the administration area.

Representative categories include:

```text id="h6q2m9"
General
Localization
Caching
Analytics
AI
Comments
Security
SEO
```

Runtime settings are persisted in the database and can be changed by an authorized administrator.

Infrastructure-level configuration and sensitive values such as connection strings, secrets, and environment-specific configuration are managed through application configuration, environment variables, and secret storage.

Therefore:

```text id="p7n4x1"
Infrastructure Configuration
        ↓
appsettings / Environment / Secrets

Runtime Settings
        ↓
Database / Administration Panel
```

These two configuration categories are intentionally kept separate.

---

## 13. Summary

DevJourney uses a deliberately simple but controlled architecture:

```text id="m8q3r7"
                 Presentation
                      │
                      ▼
                 Application
                      │
                      ▼
                    Domain
                      ▲
                      │
                Infrastructure
```

The purpose of the architecture is not to maximize the number of patterns, abstractions, or technologies. Instead, the system uses professional patterns such as Factory Method, Repository, Unit of Work, Strategy, Adapter, Decorator, and State only when they solve concrete problems.

Every architectural decision is evaluated through the trade-off between simplicity, maintainability, extensibility, and the actual requirements of the system.

DevJourney intentionally avoids unnecessary complexity such as Microservices, Message Brokers, Mediator, and mandatory CQRS because, at the current scale of the system, their operational and architectural costs outweigh their practical benefits.