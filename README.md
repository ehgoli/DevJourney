# DevJourney

> A personal developer portfolio and knowledge platform built with ASP.NET Core, designed to present professional identity, technical skills, projects, courses, career journey, and technical articles.

[![Status](https://img.shields.io/badge/status-in%20development-orange)](#project-status)
[![.NET](https://img.shields.io/badge/.NET-10-512BD4)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Razor%20Pages-512BD4)](https://learn.microsoft.com/aspnet/core/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-18-4169E1)](https://www.postgresql.org/)
[![Redis](https://img.shields.io/badge/Redis-cache-DC382D)](https://redis.io/)

---

## Overview

DevJourney is a multilingual personal website and content platform designed to present a developer's professional profile in a structured and maintainable way.

The platform combines a public portfolio website with an administration area for managing content, comments, analytics, AI-assisted features, localization, and runtime settings.

The project is intentionally designed as a **Layered Monolith** with clear architectural boundaries rather than introducing distributed architecture or infrastructure that is not justified by the current scope.

The primary goal is not to maximize the number of technologies or design patterns, but to demonstrate how professional architectural and engineering decisions can be applied to a real-world application of moderate complexity.

---

## Project Status

**In Development**

The project is being developed incrementally. Architecture, infrastructure, documentation, and application features are introduced progressively rather than being treated as separate final stages.

---

## Features

### Public Website

- Personal profile and professional introduction
- Skills and technology proficiency
- Professional timeline
- Courses and educational content
- Portfolio and project showcase
- GitHub project links
- Technical articles
- Article comments
- Contact form
- Social media links
- Responsive interface
- Persian and English support

### Content Management

The administration area provides management capabilities for:

- Profile
- Skills
- Timeline
- Projects
- Courses
- Articles
- Comments
- Analytics
- Localization
- Site settings
- AI-assisted content operations

### AI Features

AI integration is designed around practical use cases rather than making AI part of the core Domain.

Current planned capabilities include:

- AI-assisted comment moderation
- Article improvement
- Summary generation
- Title suggestions
- Tag suggestions
- SEO content assistance

### Analytics

The system collects and aggregates user interaction data such as:

- Page views
- Unique visitors
- Article views
- Reading time
- Reading progress
- Scroll depth
- Article completion
- Project views
- Course interactions
- Traffic sources
- Language distribution

The administration dashboard presents aggregated statistics through charts and reports.

### Background Processing

Background processing is used for operations such as:

- Scheduled article publication
- AI comment moderation
- Analytics aggregation
- Other delayed or background tasks

Hangfire is used where persistent scheduling is required, while lightweight background processing can use ASP.NET Core background services.

---

# Architecture

DevJourney uses a **Layered Monolithic Architecture** based on Clean Architecture principles.

```text
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

The four primary layers are:

```text
Presentation
Application
Domain
Infrastructure
```

### Presentation

Responsible for:

- Razor Pages
- Public website
- Administration area
- Routing
- Model Binding
- UI localization
- Authentication and authorization boundaries

### Application

Responsible for:

- Application Services
- DTOs
- Validation
- Mapping
- Application abstractions
- Coordination between Domain and Infrastructure

### Domain

Responsible for:

- Entities
- Value Objects
- Business Rules
- Factories
- Domain Services
- Domain Events
- Domain-specific exceptions

### Infrastructure

Responsible for:

- Database access
- EF Core
- Dapper
- Redis
- HybridCache
- AI integrations
- Background jobs
- External services
- Localization infrastructure
- Logging
- Observability
- File storage

More details are available in:

- [Architecture](docs/en/architecture.md)
- [Infrastructure](docs/en/infrastructure.md)
- [Localization](docs/en/localization.md)

---

# Technology Stack

| Area | Technology | Purpose |
|---|---|---|
| Web | ASP.NET Core | Application platform |
| UI | Razor Pages | Server-rendered web interface |
| Database | PostgreSQL | Primary relational database |
| ORM | EF Core | Primary persistence and write operations |
| SQL | Dapper | Specialized and analytical queries |
| Cache | Redis | Distributed caching |
| Cache API | HybridCache | Local + distributed caching |
| Background Jobs | Hangfire | Scheduled and persistent jobs |
| AI | External AI Provider | Moderation and content assistance |
| Localization | ASP.NET Core Localization | UI language and Culture management |
| Logging | Serilog | Structured logging |
| Observability | OpenTelemetry | Metrics and tracing |
| Validation | Application-level validation | Input and business validation |
| Testing | xUnit | Automated testing |
| Source Control | Git / GitHub | Version control and collaboration |

The infrastructure is intentionally selective. Technologies such as Message Brokers, Microservices infrastructure, Kubernetes, MongoDB, Elasticsearch, and API Gateways are not part of the current architecture because the project does not have requirements that justify their additional complexity.

---

# Multilingual Support

DevJourney supports:

```text
Persian
English
```

The multilingual system separates UI localization from Domain content localization.

### UI Localization

Interface text is managed through ASP.NET Core Localization and resource files.

### Content Localization

Profile, Project, Course, and Timeline content can have localized representations.

### Article Language

Articles are language-specific content items. A Persian article and an English article can exist as independent records.

### Language Detection

The administrator can configure:

```text
Persian
English
Automatic
```

Automatic detection considers user preference, URL, cookie, browser language, and supporting signals such as IP and Time Zone.

Users can always switch languages manually.

More information:

[Localization Architecture](docs/en/localization.md)

---

# Caching

Caching is selective rather than global.

Typical cached content includes:

```text
Homepage
Profile
Projects
Courses
Published Articles
Popular Content
Analytics Summary
Runtime Settings
```

The caching stack is:

```text
Application
    ↓
HybridCache
    ├── Memory Cache
    └── Redis
```

Cache keys for multilingual content are language-aware:

```text
homepage:fa
homepage:en

project:{id}:fa
project:{id}:en
```

Cache invalidation is performed when the underlying data changes.

More information:

[Infrastructure](docs/en/infrastructure.md)

---

# AI Integration

AI integrations are isolated behind application abstractions and implemented in Infrastructure.

Example:

```text
Application
    ↓
IAiModerationService
    ↓
AI Adapter
    ↓
External AI Provider
```

This keeps the core application independent of a specific AI vendor.

Planned capabilities include:

- Comment moderation
- Article improvement
- Summary generation
- Title suggestions
- Tag suggestions
- SEO assistance

AI suggestions do not replace human control over published content.

---

# Analytics

The analytics system records user interaction events and aggregates them into useful metrics.

Simplified flow:

```text
Browser
   ↓
Analytics Endpoint
   ↓
PostgreSQL
   ↓
Aggregation
   ↓
Cache
   ↓
Dashboard
```

The system is designed to answer questions such as:

- How many visitors does the website receive?
- Which articles are most popular?
- How long do users stay on a page?
- How much of an article do users read?
- Which projects receive the most attention?
- How does Persian traffic compare with English traffic?
- Where does website traffic originate?

---

# Design Principles

DevJourney follows a simple principle:

> **Use the simplest solution that solves the real problem, and introduce additional abstraction only when it provides measurable architectural value.**

Examples of patterns that may be used where justified include:

- Factory Method
- Repository
- Unit of Work
- Strategy
- Adapter
- Decorator
- State

Patterns are not introduced merely for demonstration purposes.

Likewise, the project intentionally avoids unnecessary architectural complexity such as:

- Microservices
- Message Brokers
- Mediator / MediatR
- Mandatory CQRS
- Generic Repository
- Overly abstract Unit of Work
- Multiple databases without a real requirement

The reasoning behind these decisions is documented in:

[Architecture & Design Decisions](docs/en/architecture.md)

---

# Repository Structure

```text
DevJourney/
│
├── src/
│   ├── DevJourney.Web/
│   ├── DevJourney.Application/
│   ├── DevJourney.Domain/
│   └── DevJourney.Infrastructure/
│
├── frontend/
│   └── public/
│
├── tests/
│   ├── DevJourney.Domain.Tests/
│   ├── DevJourney.Application.Tests/
│   └── DevJourney.IntegrationTests/
│
├── docs/en
│   ├── architecture.md
│   ├── infrastructure.md
│   └── localization.md
│
├── screenshots/
│
├── .github/
│   └── workflows/
│
├── README.md
├── Dockerfile
├── docker-compose.yml
└── DevJourney.sln
```

---

# Frontend Template

The repository also contains the frontend template used as the visual foundation of the project.

```text
frontend/
└── public
```

The template is maintained separately from the ASP.NET Core implementation so that the visual layer remains easy to inspect, reuse, and understand independently from the backend architecture.

---


# Documentation

The repository keeps technical documentation intentionally focused.

### Architecture

Describes the architecture, layer responsibilities, dependency rules, application services, design patterns, and architectural trade-offs.

[Read Architecture Documentation](docs/en/architecture.md)

### Infrastructure

Documents the selected infrastructure technologies, their responsibilities, and the reasons behind each choice.

[Read Infrastructure Documentation](docs/en/infrastructure.md)

### Localization

Documents multilingual content, UI localization, Culture detection, language selection, URL strategy, RTL/LTR behavior, and language-aware caching.

[Read Localization Documentation](docs/en/localization.md)

---

# Development

The project is currently under active development.

The initial development setup is intentionally kept simple. Environment-specific configuration, database configuration, Redis configuration, AI configuration, and other development requirements will be documented as the corresponding infrastructure is implemented.

For the current development state, refer to the project configuration and source code.

---

# License

This project is primarily published as an open-source portfolio and architectural showcase.

A final license will be defined before the first stable release.

---

# Author

**Ehsan Goli**

Backend & Full-Stack Developer

Specialized in:

```text
C#
ASP.NET Core
Software Architecture
Backend Development
Database Design
Distributed Systems Concepts
Clean Code
```

GitHub:

[github.com/ehgoli](https://github.com/ehgoli)

---

# Project Goal

DevJourney is more than a personal resume website.

It is intended to be a real-world example of how a developer can design and build a maintainable web application while making deliberate architectural decisions and avoiding unnecessary complexity.

The project focuses on:

```text
Architecture
+
Engineering Practices
+
Maintainability
+
Performance
+
Content Management
+
Observability
+
Practical AI Integration
+
Multilingual Design
```

The source code, frontend template, documentation, architectural decisions, and development process are all part of the project.