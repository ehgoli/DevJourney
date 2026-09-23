# DevJourney Infrastructure

## 1. Introduction

The infrastructure of DevJourney consists of the technologies and external integrations responsible for technical concerns outside the core Domain logic.

The primary principle behind infrastructure selection is to use the right technology for the actual problem being solved. No technology is introduced merely to increase the number of technologies in the project.

The current DevJourney infrastructure is built around:

```text
PostgreSQL
EF Core
Dapper
Redis
HybridCache
Hangfire
ASP.NET Core Localization
AI Provider
Serilog
OpenTelemetry
Health Checks
Rate Limiting
External Service Adapters
```

---

# 2. Database & Persistence

## PostgreSQL

PostgreSQL is the primary database of DevJourney.

It stores data such as:

```text
Profile
Timeline
Skills
Projects
Courses
Articles
Comments
Settings
Analytics
Administration
```

### Why PostgreSQL?

PostgreSQL was selected because it provides:

- A mature relational data model
- Strong transaction support
- Powerful SQL capabilities
- Advanced indexing
- JSON support when appropriate
- Suitable performance for the project's transactional and analytical workloads
- Strong support within the .NET ecosystem

The current version of DevJourney does not require multiple database technologies.

---

## EF Core

EF Core is the primary persistence technology in DevJourney.

Its main responsibilities include:

- Entity Mapping
- Change Tracking
- CRUD Operations
- Transactions
- Migrations
- Database Configuration

The typical flow is:

```text
Application Service
       ↓
Repository / DbContext
       ↓
EF Core
       ↓
PostgreSQL
```

EF Core is the primary technology for write operations and normal persistence workloads.

### Why EF Core?

- Natural integration with ASP.NET Core
- Reduced need for handwritten SQL for conventional operations
- Strong entity mapping capabilities
- Transaction support
- Migration support
- Good fit for the project's Domain Model

---

## Dapper

Dapper is not used as the primary persistence technology.

It is introduced selectively for queries that benefit from:

- Explicit SQL
- Specialized projections
- Complex queries
- Reporting
- Analytics
- Performance-sensitive reads

For example:

```text
AnalyticsService
       ↓
AnalyticsQueries
       ↓
Dapper
       ↓
PostgreSQL
```

The goal is to keep normal persistence simple with EF Core while using Dapper where direct SQL control provides meaningful value.

---

## Repository

Repositories are introduced only when a meaningful persistence abstraction exists for a domain concept.

Examples include:

```text
IArticleRepository
ICommentRepository
IProjectRepository
```

Their implementations reside in Infrastructure.

A Generic Repository abstraction is intentionally avoided because a universal repository does not provide enough value for this project.

---

## Unit of Work

EF Core's `DbContext` already provides Unit of Work behavior, with `SaveChanges` defining the persistence commit boundary.

Therefore, DevJourney uses `DbContext` as the default Unit of Work implementation unless there is a concrete reason to introduce a separate abstraction.

If a future requirement makes an explicit application-level transaction abstraction useful, a thin `IUnitOfWork` can be defined and implemented in Infrastructure.

A generic or heavily abstracted Unit of Work is intentionally avoided.

---

# 3. Caching

## Redis

Redis is used as the distributed cache.

Suitable cached data includes:

```text
Profile
Homepage Data
Projects
Courses
Published Articles
Popular Content
Dashboard Metrics
Runtime Settings
```

Redis provides the most value for data that:

- Is frequently requested
- Changes relatively infrequently
- Is expensive to produce
- Is repeatedly requested across different requests

---

## HybridCache

HybridCache is used to combine local memory caching with distributed caching.

The general flow is:

```text
Request
   ↓
L1 Memory Cache
   ↓ miss
L2 Redis
   ↓ miss
PostgreSQL
```

### Why HybridCache?

HybridCache helps to:

- Reduce repeated Redis access
- Serve hot data faster
- Reduce cache stampede risks
- Centralize cache handling

It is particularly suitable for data such as Homepage, Profile, and Project information.

---

## Cache Strategy

Caching is selective rather than global.

Typical candidates include:

```text
Homepage
Profile
Portfolio
Courses
Published Articles
Popular Articles
Analytics Summary
```

Data such as:

```text
Pending Comments
Contact Messages
Authentication-sensitive Data
```

should not be cached merely for the sake of performance.

---

## Cache Invalidation

Every cache policy must have an explicit invalidation strategy.

For example:

```text
Update Project
      ↓
Persist Changes
      ↓
Invalidate Project Cache
      ↓
Next Request
      ↓
Load Fresh Data
```

Cache keys are language-aware for multilingual content:

```text
homepage:fa
homepage:en

project:{id}:fa
project:{id}:en
```

This prevents localized versions from overwriting each other.

---

# 4. Background Processing

DevJourney has two distinct categories of background work:

```text
Scheduled Processing
Background Processing
```

They are not treated as the same technical problem.

---

## Hangfire

Hangfire is used for jobs that:

- Require scheduling
- Should be persisted
- May need retry behavior
- Must survive application restarts

The primary use case is:

```text
Schedule Article
      ↓
Hangfire
      ↓
Publish Article
```

Other potential uses include:

```text
Analytics Aggregation
Scheduled Maintenance
Delayed Processing
```

---

## BackgroundService

For simpler and lightweight background work, `BackgroundService` and ASP.NET Core hosted services may be used.

They are suitable for workloads that:

- Run continuously
- Do not require persistent job scheduling
- Do not require complex retry persistence

Hangfire is therefore not treated as the solution for every background operation.

---

# 5. AI Integration

AI is treated as an external infrastructure integration and resides in Infrastructure.

Application depends only on abstractions such as:

```text
IAiModerationService
IArticleWritingService
```

Actual provider implementations are located in Infrastructure.

---

## AI Comment Moderation

The general flow is:

```text
User
  ↓
Create Comment
  ↓
Comment = Pending
  ↓
Background Job
  ↓
AI Moderation
  ↓
Approved / Flagged / Rejected
```

The AI may also return:

```text
Decision
Reason
Confidence
```

Ambiguous comments can be flagged for administrator review.

AI is not part of the Domain model and the Domain must not depend on a specific AI SDK or provider.

---

## AI Article Assistant

AI is used to assist with content creation and improvement.

Potential capabilities include:

```text
Improve Article
Generate Summary
Suggest Title
Suggest Tags
Improve SEO Description
```

The flow is:

```text
Admin
   ↓
Article Writing Assistant
   ↓
Application Abstraction
   ↓
AI Adapter
   ↓
External AI Provider
```

AI generates suggestions; final content remains under the author's control.

---

# 6. Localization Infrastructure

Localization is treated as a general infrastructure capability.

Its responsibilities include:

- Resolving the current culture
- Resolving the current UI culture
- Language detection
- Managing the default language
- Connecting culture to the request pipeline
- Supporting `fa` and `en`
- Handling RTL / LTR

The general flow is:

```text
Request
   ↓
Language Resolution
   ↓
Current Culture
   ↓
Presentation
```

UI resources are stored in resource files.

Domain content such as Profile, Project, and Course content is handled according to its own translation model.

---

# 7. Language Detection

The site language policy can be configured as:

```text
Persian
English
Automatic
```

In Automatic mode, language detection follows this order:

```text
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

IP and Time Zone are supporting signals rather than authoritative language sources and must never override an explicit user preference.

---

# 8. Analytics Infrastructure

Analytics is used to measure user behavior across the public website.

Representative events include:

```text
PageView
ArticleOpened
ReadingStarted
ReadingHeartbeat
ScrollDepth
ArticleCompleted
ProjectViewed
CourseClicked
ExternalLinkClicked
CommentSubmitted
```

The general flow is:

```text
Browser
   ↓
Analytics Endpoint
   ↓
PostgreSQL
   ↓
Aggregation
   ↓
Dashboard
```

The system can calculate metrics such as:

```text
Total Visitors
Unique Visitors
Page Views
Average Session Duration
Average Reading Time
Article Completion Rate
Top Articles
Top Projects
Traffic Sources
```

Culture and Language can also be recorded with analytics events to support language-specific reporting.

---

## Analytics Aggregation

Raw events can be transformed into aggregated metrics:

```text
Raw Events
    ↓
Daily Aggregation
    ↓
Weekly Aggregation
    ↓
Monthly Aggregation
```

Aggregated results can then be cached to prevent expensive analytical queries from being executed on every dashboard request.

---

# 9. Dashboard & Reporting

The administration dashboard uses dedicated analytics queries.

For complex reporting and analytical queries, Dapper is used where appropriate:

```text
AnalyticsService
      ↓
AnalyticsQueries
      ↓
Dapper
      ↓
PostgreSQL
```

Frequently requested dashboard results can be cached through Redis / HybridCache.

The dashboard can expose metrics such as:

```text
Visitors
Page Views
Reading Time
Traffic Sources
Top Content
Language Distribution
Daily / Weekly / Monthly Trends
```

---

# 10. Security

DevJourney is a public website and includes publicly accessible operations such as comments and contact forms. Security is therefore a required part of the infrastructure.

## Authentication

Authentication serves two audiences: it protects the administration area, and it powers public user accounts (sign up, sign in, password recovery) on the public website.

Public accounts are identified by phone number rather than email. Sign-up, login, and password recovery all use the phone number as the primary identifier; password recovery is a code-based flow (phone number, verification code, new password).

## Authorization

Administrative operations are protected through appropriate authorization policies or roles.

The current scope does not require a complex role hierarchy; a dedicated Admin policy can be sufficient.

## Rate Limiting

Rate limiting is applied to public operations such as:

```text
Comment Submission
Contact Form
Analytics
```

Its goals include:

- Abuse prevention
- Spam reduction
- Burst traffic control
- Reducing pressure on the database and AI provider

## Input Validation

All user input is validated before it reaches business logic.

Validation is not limited to the UI and is also enforced within the Application Layer.

## CSRF Protection

Appropriate CSRF protection is enabled for cookie-based authenticated forms and administrative operations.

## Security Headers

Appropriate security headers, including Content Security Policy where applicable, `X-Content-Type-Options`, and other relevant headers are configured for the public website.

---

# 11. Logging

Serilog is used for structured application logging.

Where appropriate, logs may contain:

```text
Timestamp
Log Level
Message
Exception
Request Id
User Id
Operation
Duration
```

The goal is to make failures and system behavior observable without relying solely on basic console output.

Sensitive values such as passwords, tokens, and secrets must never be logged.

---

# 12. Observability

OpenTelemetry is used for metrics and tracing where observability provides meaningful value.

Primary areas include:

```text
HTTP Requests
Database Operations
External AI Calls
Background Jobs
Application Errors
```

The goal is to understand the execution path and identify bottlenecks such as:

```text
Request
  ↓
Application Service
  ↓
Database / Redis / External Service
```

including timing information and failure locations.

---

# 13. Health Checks

Health Checks are used to monitor the state of the application and its critical dependencies.

Typical checks include:

```text
Application
PostgreSQL
Redis
```

Health monitoring should distinguish between:

```text
Application is running
```

and:

```text
Application is healthy
```

---

# 14. External Services

External services are never consumed directly from the Application Layer.

Each external integration is isolated behind an Adapter in Infrastructure.

For example:

```text
Application
    ↓
IEmailService
    ↓
EmailAdapter
    ↓
Email Provider
```

or:

```text
Application
    ↓
IGitHubService
    ↓
GitHubAdapter
    ↓
GitHub API
```

The purpose of these abstractions is to prevent Application and Domain code from becoming dependent on external SDKs and APIs.

---

# 15. File & Media Storage

DevJourney contains media such as:

```text
Profile Images
Project Covers
Course Covers
Article Images
```

These are accessed through an abstraction such as:

```text
IFileStorage
```

The initial implementation may use local file storage:

```text
Application
    ↓
IFileStorage
    ↓
LocalFileStorage
```

If requirements change later, the storage provider can be replaced without changing the Business Logic.

---

# 16. Configuration

Configuration is divided into two primary categories.

### Infrastructure Configuration

Environment-specific or deployment-related values such as:

```text
Database Connection
Redis Connection
AI API Keys
Email Credentials
Security Secrets
```

These are supplied through:

```text
appsettings
Environment Variables
Secrets
```

### Runtime Settings

Settings that administrators can manage through the administration panel:

```text
General
Localization
Caching
Analytics
AI
Comments
Security
SEO
```

These values are stored in the database.

The two configuration categories are intentionally separated.

---

# 17. Settings Cache

Runtime settings may be cached to reduce repeated database reads.

The flow is:

```text
Settings Request
       ↓
Cache
   ↓ miss
Database
       ↓
Update Cache
```

When an administrator changes a setting:

```text
Admin
 ↓
Update Settings
 ↓
Database
 ↓
Invalidate Settings Cache
```

The next request then retrieves the updated value.

---

# 18. SEO Infrastructure

Because DevJourney is a public multilingual website, SEO is treated as an important presentation/infrastructure concern.

The system can support:

```text
Localized URLs
Canonical URLs
Meta Title
Meta Description
Open Graph
Sitemap
Robots.txt
hreflang
Structured Data
```

Language-aware URLs are especially important for multilingual content.

---

# 19. Performance Principles

Performance in DevJourney is addressed through targeted optimizations rather than by adding more infrastructure technologies.

Primary principles include:

```text
Database Indexing
Efficient Queries
Selective Caching
Pagination
Avoiding Unnecessary Data Loading
Dapper for Complex Reads
Background Processing
Rate Limiting
```

Optimization should be driven by measurement and actual bottlenecks rather than assumptions.

Caching or query optimization should not be introduced purely because they are technically available.

---

# 20. Intentionally Excluded Infrastructure

The following technologies are intentionally not part of the current DevJourney infrastructure:

```text
RabbitMQ
Kafka
Message Brokers
Microservices Infrastructure
API Gateway
Kubernetes
Elasticsearch
MongoDB
Additional Distributed Datastores
```

Their exclusion does not imply that these technologies are unsuitable in general.

They are excluded because the current scope of DevJourney does not contain problems that justify their additional operational and architectural complexity.

The infrastructure should grow with the problem rather than anticipating complexity that does not yet exist.

---

# 21. Infrastructure Summary

The DevJourney infrastructure stack is currently centered around:

```text
Web
    ASP.NET Core Razor Pages

Database
    PostgreSQL

ORM
    EF Core

Specialized Queries
    Dapper

Caching
    Redis + HybridCache

Background Jobs
    Hangfire
    BackgroundService

AI
    External AI Provider + Adapter

Localization
    ASP.NET Core Localization

Logging
    Serilog

Observability
    OpenTelemetry

Health
    ASP.NET Core Health Checks

Security
    Authentication
    Authorization
    Rate Limiting
    Validation
    CSRF Protection

External Integrations
    Adapter-based

File Storage
    IFileStorage + Local Provider
```

All of these choices follow the same principle:

```text
Real Problem
     ↓
Appropriate Technology
     ↓
Minimum Necessary Complexity
```

The infrastructure of DevJourney is not intended to become a technology showcase. Its purpose is to support the real capabilities of the application using standard, maintainable technologies that are appropriate for the project's current scale.