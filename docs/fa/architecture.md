# معماری DevJourney

## 1. مقدمه

DevJourney یک پلتفرم شخصی برای معرفی هویت حرفه‌ای، مهارت‌ها، مسیر فعالیت، دوره‌های آموزشی، پروژه‌ها و مقالات است. علاوه بر بخش عمومی سایت، یک پنل مدیریت برای مدیریت محتوا، تنظیمات، نظرات و داده‌های تحلیلی در نظر گرفته شده است.

هدف معماری DevJourney ایجاد سیستمی است که در عین سادگی متناسب با ابعاد واقعی پروژه، قابل نگهداری، توسعه‌پذیر، تست‌پذیر و از نظر فنی قابل دفاع باشد.

در طراحی این پروژه، پیچیدگی معماری به‌عنوان یک هدف مستقل در نظر گرفته نشده است. هر تکنولوژی، الگو یا abstraction فقط زمانی استفاده می‌شود که مسئله مشخصی را حل کند و مزیت آن از هزینه پیچیدگی بیشتر باشد.

---

## 2. معماری انتخاب‌شده

DevJourney از یک **Layered Monolithic Architecture** با اصول **Clean Architecture** استفاده می‌کند.

لایه‌های اصلی سیستم:

```text
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

چهار لایه اصلی:

```text
Presentation
Application
Domain
Infrastructure
```

سیستم یک Monolith است و تمام بخش‌های آن در قالب یک Application واحد اجرا می‌شوند. با توجه به ماهیت و اندازه پروژه، استفاده از Microservices یا Modular Monolith باعث ایجاد پیچیدگی عملیاتی و معماری بیشتری نسبت به نیاز واقعی سیستم می‌شد.

---

## 3. اهداف معماری

معماری DevJourney بر اهداف زیر بنا شده است:

- Separation of Concerns
- Dependency Inversion
- Maintainability
- Testability
- Extensibility
- Simplicity
- Explicit Business Logic
- Controlled Infrastructure Dependencies
- قابلیت توسعه بدون وابستگی شدید به یک Provider یا تکنولوژی خاص

معماری باید به توسعه‌دهنده کمک کند سیستم را راحت‌تر درک و تغییر دهد، نه اینکه صرفاً تعداد لایه‌ها یا abstractionهای سیستم را افزایش دهد.

---

## 4. مسئولیت لایه‌ها

### 4.1 Presentation

Presentation نقطه ورود کاربران و تعامل آن‌ها با سیستم است.

این لایه با **ASP.NET Core Razor Pages** پیاده‌سازی می‌شود.

مسئولیت‌های اصلی:

- نمایش صفحات عمومی
- نمایش پنل مدیریت
- دریافت ورودی کاربر
- Model Binding
- نمایش Validation Errors
- Authentication / Authorization در سطح Presentation
- مدیریت Routing
- Localization در سطح UI
- نمایش View Modelها
- تعامل با Application Layer

Presentation نباید منطق اصلی Business را پیاده‌سازی کند.

برای مثال:

```text
Razor Page
    ↓
IArticleService
    ↓
Application
```

به‌جای اینکه مستقیم با `DbContext`، Redis یا AI Provider کار کند.

---

### 4.2 Application

Application محل اجرای Use Caseهای برنامه و orchestration بین Domain و Infrastructure است.

ساختار این لایه به‌صورت Type-Based سازمان‌دهی می‌شود:

```text
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

Application Serviceهای کلاسیک برای عملیات اصلی سیستم استفاده می‌شوند.

نمونه:

```text
ArticleService
CommentService
ProjectService
CourseService
AnalyticsService
SettingsService
```

Application می‌تواند از abstractionهایی مانند Repository، AI Service یا Cache استفاده کند، بدون اینکه به implementationهای Infrastructure وابسته شود.

---

### 4.3 Domain

Domain قلب سیستم است و باید مستقل از تکنولوژی‌های زیرساختی باقی بماند.

مسئولیت‌های Domain:

- Entities
- Value Objects
- Enums
- Business Rules
- Domain Services
- Factories
- Domain Events
- Domain Exceptions

Domain نباید به این موارد وابستگی مستقیم داشته باشد:

```text
EF Core
Dapper
Redis
Hangfire
AI SDK
Razor Pages
ASP.NET Core
```

منطق Business تا حد امکان در Domain قرار می‌گیرد و Application فقط جریان اجرای آن را orchestrate می‌کند.

---

### 4.4 Infrastructure

Infrastructure مسئول تمام جزئیات فنی و Integrationهای خارجی است.

نمونه ساختار:

```text
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

این لایه می‌تواند implementationهای abstractionهای تعریف‌شده در Application را ارائه کند.

---

## 5. Dependency Rules

قانون اصلی Dependency در DevJourney این است که Domain نباید به بیرون وابسته باشد.

```text
Presentation
      ↓
Application
      ↓
Domain
```

Infrastructure از abstractionهای Application استفاده می‌کند:

```text
Infrastructure
      ↓
Application
      ↓
Domain
```

بنابراین Application نباید مستقیماً به implementationهای Infrastructure وابسته باشد.

برای مثال:

```text
Application
    ↓
IArticleRepository
```

و:

```text
Infrastructure
    ↓
ArticleRepository : IArticleRepository
```

به این ترتیب Business Logic از جزئیات persistence مستقل باقی می‌ماند.

---

# 6. سازمان‌دهی Type-Based

DevJourney به‌صورت کلی از **Type-Based Organization** استفاده می‌کند.

برای مثال:

```text
Application/
├── Services/
├── DTOs/
├── Validators/
└── Interfaces/
```

به‌جای اینکه برای هر Use Case مجموعه‌ای از Handlerها و فایل‌های مستقل ایجاد شود.

این تصمیم به دلیل اندازه پروژه و تعداد محدود Business Capabilityها اتخاذ شده است.

برای این پروژه، Serviceهای کلاسیک باعث می‌شوند کد ساده‌تر، قابل کشف‌تر و قابل نگهداری‌تر باقی بماند.

---

# 7. Application Services

DevJourney از Serviceهای کلاسیک Application استفاده می‌کند و از Mediator برای عبور دادن درخواست‌ها بین Presentation و Application استفاده نمی‌شود.

نمونه:

```text
Presentation
      ↓
IArticleService
      ↓
ArticleService
      ↓
Domain / Persistence
```

Serviceها باید بر اساس Business Responsibility سازمان‌دهی شوند و نباید به God Service تبدیل شوند.

برای مثال:

```text
ArticleService
CommentService
AnalyticsService
SettingsService
```

هر Service مسئول یک حوزه مشخص است.

تا زمانی که یک Service اندازه و پیچیدگی معقولی داشته باشد، به Use Caseهای بسیار ریز یا Handlerهای متعدد تقسیم نمی‌شود.

---

# 8. Design Patterns

Patternها در DevJourney برای نمایش مهارت یا افزایش مصنوعی پیچیدگی استفاده نمی‌شوند.

هر Pattern زمانی استفاده می‌شود که مسئله مشخصی را بهتر حل کند.

الگوی تصمیم‌گیری:

```text
Problem
   ↓
Simple Solution
   ↓
Does a Pattern provide real value?
   ↓
Yes → Use Pattern
No  → Keep the simple solution
```

### Factory Method

Factory Method زمانی استفاده می‌شود که ایجاد یک Entity شامل Validation، Invariant یا Creation Logic معنادار باشد.

برای مثال:

```text
ArticleFactory
CommentFactory
```

نباید برای Entityهای ساده صرفاً با هدف استفاده از Pattern Factory ساخته شوند.

### Repository

Repository برای Aggregateهایی که Persistence abstraction واقعی برای آن‌ها ارزش دارد استفاده می‌شود.

Repositoryها اختصاصی هستند و Generic Repository در سیستم ایجاد نمی‌شود.

مثلاً:

```text
IArticleRepository
ICommentRepository
IProjectRepository
```

### Unit of Work

در بخش Persistence، `DbContext` خود EF Core رفتار Unit of Work را ارائه می‌دهد.

در صورت نیاز به یک abstraction مستقل برای Transaction Boundary، یک `IUnitOfWork` باریک می‌تواند در Application تعریف و در Infrastructure پیاده‌سازی شود.

Generic Unit of Work یا abstractionهای چندلایه برای هماهنگ کردن Repositoryها ایجاد نمی‌شود.

### Strategy

Strategy زمانی استفاده می‌شود که چند implementation واقعی از یک رفتار داشته باشیم.

یک نمونه مناسب، AI Providerها یا Providerهای خارجی قابل تعویض است:

```text
IAiModerationService
        │
        ├── Provider A
        └── Provider B
```

### Adapter

برای جدا کردن سیستم از APIها و SDKهای خارجی استفاده می‌شود.

مثلاً:

```text
GitHub API
AI Provider
Email Provider
```

Application فقط abstraction را مشاهده می‌کند.

### Decorator

Decorator در صورت نیاز می‌تواند برای Cross-Cutting Concernهایی مانند Caching استفاده شود.

برای مثال:

```text
IArticleService
      ↑
CachedArticleService
      ↑
ArticleService
```

این الگو تنها زمانی اضافه می‌شود که جداسازی Cache از Business Service واقعاً ارزش داشته باشد.

### State

Article دارای Lifecycle مشخص است:

```text
Draft
Scheduled
Published
Archived
```

در صورت پیچیده شدن رفتار Stateها، State Pattern می‌تواند مناسب باشد. اگر State Logic ساده باقی بماند، استفاده از یک Enum و Business Rules معمولی ترجیح داده می‌شود.

---

# 9. Architectural Decisions & Trade-offs

## Layered Monolith به‌جای Microservices

DevJourney یک سایت شخصی و یک Application متمرکز است. تقسیم آن به چند Microservice باعث اضافه شدن مواردی مانند Network Communication، Distributed Failure، Service Deployment و Data Consistency می‌شد، بدون اینکه مسئله واقعی متناسبی برای این هزینه وجود داشته باشد.

بنابراین یک Monolith لایه‌ای انتخاب شده است.

## عدم استفاده از Message Broker

RabbitMQ و سایر Message Brokerها در نسخه فعلی پروژه استفاده نمی‌شوند.

کارهای asynchronous و scheduled با ابزارهای داخلی و Background Processing انجام می‌شوند.

برای این پروژه، Message Broker پیچیدگی بیشتری نسبت به ارزش واقعی آن ایجاد می‌کرد.

## عدم استفاده از Mediator

MediatR و الگوی Mediator به پروژه اضافه نشده‌اند.

Application Serviceهای کلاسیک برای تعداد فعلی Use Caseها کافی هستند و یک لایه indirection اضافی ایجاد نمی‌کنند.

## عدم استفاده از CQRS اجباری

مدل Command/Query کاملاً جدا برای تمام عملیات سیستم استفاده نمی‌شود.

Queryهای پیچیده در صورت نیاز می‌توانند با Dapper پیاده‌سازی شوند، اما سیستم به‌صورت کامل CQRS محور نیست.

## عدم استفاده از Generic Repository

Generic Repository abstraction یکسانی برای Entityها ایجاد نمی‌شود.

Repositoryها در صورت وجود، بر اساس نیاز واقعی Domain و Persistence طراحی می‌شوند.

## انتخاب PostgreSQL

PostgreSQL به‌عنوان Database اصلی انتخاب شده است.

دلایل:

- Relational Model مناسب برای Domain
- Queryهای قدرتمند
- Transaction Support
- Indexing
- JSON Support در صورت نیاز
- مناسب بودن برای Analytics سبک و متوسط
- پشتیبانی مناسب در اکوسیستم .NET

## انتخاب EF Core

EF Core ابزار اصلی Persistence و Write Operations است.

دلایل:

- Integration مناسب با ASP.NET Core
- Change Tracking
- Transaction Support
- Migrations
- Mapping مناسب Domain Entities
- کاهش نیاز به SQL دستی در عملیات معمول

## انتخاب Dapper

Dapper ابزار اصلی سیستم نیست و فقط برای Queryهایی استفاده می‌شود که نیاز به SQL صریح، Projectionهای خاص یا Queryهای تحلیلی پیچیده دارند.

این رویکرد باعث می‌شود از EF Core برای عملیات معمول و از Dapper برای Queryهای تخصصی استفاده شود، بدون اینکه کل سیستم به دو ORM وابسته شود.

## انتخاب Redis و HybridCache

Cache برای داده‌های پرتکرار و کم‌تغییر مانند Profile، Projects، Courses، Published Articles و Dashboard Metrics استفاده می‌شود.

HybridCache امکان استفاده همزمان از Local Memory Cache و Distributed Cache را فراهم می‌کند و در نتیجه برای یک Application کوچک تا متوسط انتخاب مناسبی است.

Redis نیز به‌عنوان Distributed Cache مورد استفاده قرار می‌گیرد.

## انتخاب Hangfire

Hangfire برای Jobهایی که نیاز به Scheduling و Persistence دارند استفاده می‌شود.

نمونه:

```text
Publish Scheduled Article
```

کارهای ساده‌تر Background می‌توانند با BackgroundService یا Workerهای داخلی انجام شوند.

## انتخاب AI به‌عنوان Infrastructure Integration

AI بخشی از Domain نیست.

Application فقط abstraction مورد نیاز را می‌شناسد:

```text
IAiModerationService
IArticleWritingService
```

و Provider واقعی در Infrastructure قرار می‌گیرد.

این طراحی باعث می‌شود Provider در آینده قابل تغییر باشد.

---

# 10. قابلیت چندزبانه

Multilingual Support یکی از قابلیت‌های زیرساختی سیستم است و به‌صورت سراسری در پروژه طراحی می‌شود.

اما مدل زبان در Domain با توجه به نوع Entity متفاوت است.

### UI Localization

متن‌های ثابت رابط کاربری توسط ASP.NET Core Localization و Resource Files مدیریت می‌شوند.

```text
fa
en
```

### Content Localization

داده‌هایی مانند Profile، Project، Course و Timeline که یک موجودیت واحد با دو نمایش زبانی هستند، دارای Translation Model هستند.

مثلاً:

```text
Project
├── Common Data
└── ProjectTranslation
    ├── fa
    └── en
```

### Article Language

Article به‌عنوان یک Content مستقل دارای Language است.

یعنی یک Article فارسی و یک Article انگلیسی می‌توانند دو رکورد مستقل باشند.

```text
Article
├── Language = fa
```

یا:

```text
Article
├── Language = en
```

این تصمیم با ماهیت مستقل مقالات متناسب است.

---

# 11. Language Detection

Admin می‌تواند سیاست پیش‌فرض زبان را تعیین کند:

```text
Persian
English
Automatic
```

در حالت Automatic، سیستم زبان مناسب را با ترتیب زیر تشخیص می‌دهد:

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

انتخاب صریح کاربر همیشه اولویت بالاتری از تشخیص خودکار دارد.

کاربر نیز می‌تواند بدون توجه به زبان تشخیص داده‌شده، زبان دیگر را انتخاب کند.

---

# 12. Settings

تنظیمات Runtime توسط پنل مدیریت قابل کنترل هستند.

نمونه دسته‌ها:

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

تنظیمات Runtime در Database نگهداری می‌شوند.

در مقابل، تنظیمات زیرساختی و حساس مانند Connection Stringها، Secretها و تنظیمات Deployment از Configuration و Environment Variables دریافت می‌شوند.

بنابراین:

```text
Deployment Configuration
        ↓
appsettings / Environment / Secrets

Runtime Settings
        ↓
Database / Admin Panel
```

این دو مفهوم عمداً از هم جدا نگه داشته می‌شوند.

---

# 13. نتیجه

DevJourney از یک معماری ساده اما کنترل‌شده استفاده می‌کند:

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

هدف این معماری اضافه کردن Pattern و Technology به‌عنوان Showcase نیست؛ هدف ایجاد یک سیستم قابل فهم و قابل توسعه است که در نقاط لازم بتواند از ابزارها و الگوهای حرفه‌ای مانند Factory Method، Repository، Unit of Work، Strategy، Adapter و Decorator استفاده کند.

هر تصمیم معماری بر اساس Trade-off بین سادگی، قابلیت نگهداری، توسعه‌پذیری و نیاز واقعی سیستم اتخاذ می‌شود.

DevJourney عمداً از پیچیدگی‌هایی مانند Microservices، Message Broker، Mediator و CQRS اجباری اجتناب می‌کند، زیرا در ابعاد فعلی سیستم ارزش آن‌ها کمتر از هزینه‌ای است که به معماری و عملیات اضافه می‌کنند.