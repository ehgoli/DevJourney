# زیرساخت DevJourney

## 1. مقدمه

زیرساخت DevJourney مجموعه‌ای از فناوری‌ها و سرویس‌هایی است که برای پیاده‌سازی جزئیات فنی خارج از منطق اصلی Domain استفاده می‌شوند.

اصل اصلی در انتخاب زیرساخت این پروژه، استفاده از فناوری مناسب برای مسئله واقعی است. هیچ فناوری صرفاً برای افزایش تعداد تکنولوژی‌های پروژه اضافه نمی‌شود.

زیرساخت DevJourney بر پایه موارد زیر شکل گرفته است:

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

PostgreSQL پایگاه داده اصلی DevJourney است.

داده‌هایی مانند موارد زیر در PostgreSQL نگهداری می‌شوند:

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

### دلیل انتخاب PostgreSQL

PostgreSQL برای این پروژه انتخاب شده است زیرا:

- یک دیتابیس رابطه‌ای قدرتمند و mature است.
- Transaction Support مناسبی دارد.
- SQL و قابلیت‌های Query آن برای Analytics مناسب هستند.
- Indexing قدرتمندی ارائه می‌دهد.
- در صورت نیاز قابلیت‌های JSON نیز در دسترس هستند.
- برای حجم و ماهیت داده‌های DevJourney کاملاً کافی است.
- در اکوسیستم .NET و EF Core پشتیبانی بسیار خوبی دارد.

در نسخه فعلی پروژه نیازی به استفاده هم‌زمان از چند Database وجود ندارد.

---

## EF Core

EF Core فناوری اصلی Persistence در DevJourney است.

وظایف اصلی:

- Entity Mapping
- Change Tracking
- CRUD Operations
- Transactions
- Migrations
- Database Configuration

جریان معمول:

```text
Application Service
       ↓
Repository / DbContext
       ↓
EF Core
       ↓
PostgreSQL
```

EF Core انتخاب اصلی برای عملیات Write و Persistence معمول سیستم است.

### دلیل انتخاب EF Core

- Integration طبیعی با ASP.NET Core
- کاهش نیاز به SQL دستی برای عملیات معمول
- Mapping مناسب Entityها
- پشتیبانی از Transactions
- پشتیبانی از Migrations
- مناسب بودن برای Domain Model

---

## Dapper

Dapper به‌عنوان ابزار اصلی Persistence استفاده نمی‌شود.

در Queryهایی استفاده می‌شود که نیازمند یکی از موارد زیر هستند:

- SQL صریح
- Projectionهای تخصصی
- Queryهای پیچیده
- Reporting
- Analytics
- Queryهای Performance-sensitive

مثلاً:

```text
AnalyticsService
       ↓
AnalyticsQueries
       ↓
Dapper
       ↓
PostgreSQL
```

هدف این است که EF Core برای اکثر عملیات کافی باشد و Dapper تنها زمانی وارد شود که کنترل SQL یا Performance واقعاً مزیت ایجاد کند.

---

## Repository

Repositoryهای پروژه در صورت وجود، اختصاصی و مبتنی بر نیاز واقعی Domain هستند.

مثلاً:

```text
IArticleRepository
ICommentRepository
IProjectRepository
```

Implementation آن‌ها در Infrastructure قرار می‌گیرد.

Generic Repository برای تمام Entityها ایجاد نمی‌شود، زیرا abstraction مشترک آن الزاماً ارزش کافی ایجاد نمی‌کند.

---

## Unit of Work

EF Core `DbContext` به‌صورت ذاتی رفتار Unit of Work را فراهم می‌کند و `SaveChanges` مرز Commit تغییرات را مشخص می‌کند.

بنابراین DevJourney تا زمانی که نیاز مشخصی برای abstraction مستقل وجود نداشته باشد، از `DbContext` به‌عنوان Unit of Work استفاده می‌کند.

اگر در آینده یک Application-to-Infrastructure boundary صریح برای Transaction Management ضروری شود، یک `IUnitOfWork` باریک قابل اضافه شدن است.

از ایجاد Generic Unit of Work یا abstractionهای پیچیده اجتناب می‌شود.

---

# 3. Caching

## Redis

Redis به‌عنوان Distributed Cache پروژه استفاده می‌شود.

داده‌های مناسب برای Cache شامل:

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

Redis زمانی بیشترین ارزش را دارد که داده‌ها:

- پرتکرار باشند.
- تغییرات کمی داشته باشند.
- تولید آن‌ها نسبتاً پرهزینه باشد.
- در چند Request تکرار شوند.

---

## HybridCache

HybridCache برای ترکیب Local Memory Cache و Distributed Cache استفاده می‌شود.

الگوی کلی:

```text
Request
   ↓
L1 Memory Cache
   ↓ miss
L2 Redis
   ↓ miss
PostgreSQL
```

### دلیل استفاده

HybridCache کمک می‌کند:

- تعداد درخواست‌های مستقیم به Redis کاهش پیدا کند.
- داده‌های پرتکرار سریع‌تر پاسخ داده شوند.
- Cache Stampede بهتر کنترل شود.
- پیاده‌سازی Cache ساده‌تر و متمرکزتر باشد.

HybridCache برای داده‌های پرتکرار مانند Homepage، Profile و Projectها گزینه مناسبی است.

---

## Cache Strategy

Cache به‌صورت انتخابی اعمال می‌شود.

داده‌های زیر معمولاً قابلیت Cache شدن دارند:

```text
Homepage
Profile
Portfolio
Courses
Published Articles
Popular Articles
Analytics Summary
```

داده‌هایی مانند:

```text
Pending Comments
Contact Messages
Authentication-sensitive Data
```

نباید صرفاً برای افزایش Performance Cache شوند.

---

## Cache Invalidation

هر Cache Policy باید همراه با سیاست Invalidation آن تعریف شود.

مثلاً:

```text
Update Project
      ↓
Persist Changes
      ↓
Invalidate project cache
      ↓
Next request
      ↓
Load fresh data
```

برای داده‌های چندزبانه Cache Key باید Language-aware باشد:

```text
homepage:fa
homepage:en

project:{id}:fa
project:{id}:en
```

در نتیجه نسخه فارسی و انگلیسی یکدیگر را overwrite نمی‌کنند.

---

# 4. Background Processing

DevJourney به دو نوع Background Processing نیاز دارد:

```text
Scheduled Processing
Background Processing
```

این دو از نظر نیاز فنی یکسان نیستند.

---

## Hangfire

Hangfire برای کارهایی استفاده می‌شود که:

- زمان‌بندی دارند.
- باید در Database/Storage پایدار باقی بمانند.
- نیازمند Retry هستند.
- ممکن است Application در زمان اجرای Job Restart شود.

مهم‌ترین Use Case فعلی:

```text
Schedule Article
      ↓
Hangfire
      ↓
Publish Article
```

موارد دیگر می‌توانند شامل:

```text
Analytics Aggregation
Scheduled Maintenance
Delayed Processing
```

باشند.

---

## BackgroundService

برای کارهای ساده‌تر و سبک‌تر که نیازمند Persistence مستقل Job نیستند، `BackgroundService` یا Hosted Serviceهای ASP.NET Core مناسب هستند.

این روش برای کارهایی مناسب است که:

- Continuous باشند.
- Stateful Job Scheduling نیاز نداشته باشند.
- Retry Persistence پیچیده نخواهند.

بنابراین Hangfire به‌عنوان راه‌حل همه Background Workها استفاده نمی‌شود.

---

# 5. AI Integration

AI در DevJourney یک External Integration محسوب می‌شود و در Infrastructure قرار می‌گیرد.

Application فقط abstraction مورد نیاز را می‌شناسد:

```text
IAiModerationService
IArticleWritingService
```

Implementationهای واقعی در Infrastructure قرار دارند.

---

## AI Comment Moderation

فرآیند کلی:

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

AI می‌تواند علاوه بر Decision، اطلاعاتی مانند:

```text
Decision
Reason
Confidence
```

تولید کند.

موارد مشکوک می‌توانند برای بررسی Admin علامت‌گذاری شوند.

AI مستقیماً بخشی از Domain Model نیست و Domain نباید به SDK یا Provider خاصی وابسته باشد.

---

## AI Article Assistant

AI برای کمک به تولید و بهبود محتوا استفاده می‌شود.

Use Caseهای احتمالی:

```text
Improve Article
Generate Summary
Suggest Title
Suggest Tags
Improve SEO Description
```

فرآیند:

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

AI پیشنهاد تولید می‌کند و محتوای نهایی تحت کنترل نویسنده باقی می‌ماند.

---

# 6. Localization Infrastructure

Localization یکی از قابلیت‌های عمومی زیرساخت است.

مسئولیت آن شامل:

- تعیین Current Culture
- تعیین Current UI Culture
- Language Detection
- مدیریت زبان پیش‌فرض
- اتصال Culture به Request
- پشتیبانی از `fa` و `en`
- مدیریت RTL / LTR

است.

ساختار کلی:

```text
Request
   ↓
Language Resolution
   ↓
Current Culture
   ↓
Presentation
```

منابع UI در Resource Files قرار می‌گیرند.

محتوای Domain مانند Profile، Project و Course بر اساس مدل Translation خودشان مدیریت می‌شوند.

---

# 7. Language Detection

تنظیم زبان سایت می‌تواند سه حالت داشته باشد:

```text
Persian
English
Automatic
```

در Automatic Mode، اولویت تشخیص به صورت زیر است:

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

IP و Time Zone صرفاً اطلاعات کمکی برای Detection هستند و نباید انتخاب صریح کاربر را override کنند.

---

# 8. Analytics Infrastructure

Analytics برای اندازه‌گیری رفتار کاربران در سایت استفاده می‌شود.

رویدادهای نمونه:

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

جریان:

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

اطلاعات قابل استخراج:

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

Culture و Language نیز در Eventهای مربوط به رفتار کاربر ثبت می‌شوند تا گزارش‌های زبان‌محور امکان‌پذیر باشند.

---

## Analytics Aggregation

Raw Eventها می‌توانند به Metricهای قابل استفاده تبدیل شوند.

برای مثال:

```text
Raw Events
    ↓
Daily Aggregation
    ↓
Weekly Aggregation
    ↓
Monthly Aggregation
```

نتیجه Aggregation می‌تواند Cache شود تا Dashboard برای هر Request Queryهای سنگین را دوباره اجرا نکند.

---

# 9. Dashboard & Reporting

Admin Dashboard از Queryهای تخصصی Analytics استفاده می‌کند.

برای Queryهای پیچیده و Reporting، Dapper گزینه اصلی خواهد بود:

```text
AnalyticsService
      ↓
AnalyticsQueries
      ↓
Dapper
      ↓
PostgreSQL
```

نتایج پرتکرار Dashboard می‌توانند توسط Redis/HybridCache Cache شوند.

Dashboard می‌تواند شامل:

```text
Visitors
Page Views
Reading Time
Traffic Sources
Top Content
Language Distribution
Daily / Weekly / Monthly Trends
```

باشد.

---

# 10. Security

DevJourney یک سایت عمومی است و بخش‌هایی مانند Comment و Contact کاملاً قابل دسترسی هستند؛ بنابراین Security یک بخش ضروری از زیرساخت است.

### Authentication

Authentication دو مخاطب دارد: هم برای ورود به Administration Area استفاده می‌شود، و هم زیرساخت حساب‌های کاربری عمومی (ثبت‌نام، ورود، بازیابی رمز عبور) در وب‌سایت عمومی را فراهم می‌کند.

شناسه اصلی حساب‌های عمومی، شماره تلفن است، نه ایمیل. ثبت‌نام، ورود و بازیابی رمز عبور همگی بر پایه شماره تلفن هستند؛ بازیابی رمز عبور به‌صورت یک فرآیند مبتنی بر کد (شماره تلفن، کد تأیید، رمز عبور جدید) انجام می‌شود.

### Authorization

دسترسی‌های مدیریتی توسط Authorization Policyها یا Roleهای مناسب محافظت می‌شوند.

در نسخه فعلی Roleهای پیچیده لازم نیستند و یک Admin Policy می‌تواند کافی باشد.

### Rate Limiting

Rate Limiting برای Endpointها یا عملیات عمومی مانند:

```text
Comment Submission
Contact Form
Analytics
```

اعمال می‌شود.

هدف:

- جلوگیری از Abuse
- کاهش Spam
- کنترل Burst Traffic
- کاهش فشار روی Database و AI Provider

### Input Validation

تمام ورودی‌های کاربر قبل از رسیدن به Business Logic Validation می‌شوند.

این Validation فقط به UI محدود نمی‌شود و در Application نیز اعمال می‌شود.

### CSRF Protection

در فرم‌های مبتنی بر Cookie/Authentication، محافظت مناسب در برابر CSRF فعال خواهد بود.

### Security Headers

Headerهای امنیتی مناسب مانند Content Security Policy در صورت نیاز، X-Content-Type-Options و سایر Headerهای مناسب برای سایت عمومی پیکربندی می‌شوند.

---

# 11. Logging

Serilog به‌عنوان سیستم Structured Logging مورد استفاده قرار می‌گیرد.

Logها باید اطلاعاتی مانند موارد زیر را در صورت امکان ثبت کنند:

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

هدف، امکان تشخیص خطا و بررسی رفتار Application بدون وابستگی به Console Output ساده است.

اطلاعات حساس مانند Password، Token و Secret نباید Log شوند.

---

# 12. Observability

OpenTelemetry برای Metrics و Tracing در بخش‌هایی که واقعاً ارزش مشاهده‌پذیری دارند استفاده می‌شود.

تمرکز اصلی:

```text
HTTP Requests
Database Operations
External AI Calls
Background Jobs
Application Errors
```

هدف Observability این است که مشخص شود:

```text
Request
  ↓
Application Service
  ↓
Database / Redis / External Service
```

چه مدت طول کشیده و مشکل احتمالی در کدام بخش رخ داده است.

---

# 13. Health Checks

Health Checks برای بررسی وضعیت Application و Dependencies استفاده می‌شوند.

موارد مورد بررسی می‌توانند شامل:

```text
Application
PostgreSQL
Redis
```

باشند.

Health Check باید بتواند تفاوت بین:

```text
Application is running
```

و:

```text
Application is healthy
```

را مشخص کند.

---

# 14. External Services

سرویس‌های خارجی مستقیم در Application مصرف نمی‌شوند.

برای هر Integration خارجی یک Adapter در Infrastructure ایجاد می‌شود.

نمونه:

```text
Application
    ↓
IEmailService
    ↓
EmailAdapter
    ↓
Email Provider
```

یا:

```text
Application
    ↓
IGitHubService
    ↓
GitHubAdapter
    ↓
GitHub API
```

هدف این abstractionها جلوگیری از وابستگی Application و Domain به SDKها و APIهای خارجی است.

---

# 15. File & Media Storage

DevJourney شامل تصاویر Profile، Projects، Courses و Articles است.

برای این داده‌ها یک abstraction مانند:

```text
IFileStorage
```

در Application تعریف می‌شود.

Implementation فعلی می‌تواند از Local File Storage استفاده کند.

ساختار منطقی:

```text
Application
    ↓
IFileStorage
    ↓
LocalFileStorage
```

در صورت نیاز در آینده می‌توان Storage Provider را بدون تغییر Business Logic جایگزین کرد.

---

# 16. Configuration

Configuration به دو دسته اصلی تقسیم می‌شود:

### Infrastructure Configuration

اطلاعاتی که به Environment یا Deployment وابسته‌اند:

```text
Database Connection
Redis Connection
AI API Keys
Email Credentials
Security Secrets
```

این اطلاعات از:

```text
appsettings
Environment Variables
Secrets
```

دریافت می‌شوند.

### Runtime Settings

تنظیماتی که Admin می‌تواند در پنل مدیریت تغییر دهد:

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

این تنظیمات در Database نگهداری می‌شوند.

این دو سیستم عمداً از هم جدا هستند.

---

# 17. Settings Cache

تنظیمات Runtime می‌توانند برای کاهش Readهای تکراری Cache شوند.

جریان:

```text
Settings Request
       ↓
Cache
   ↓ miss
Database
       ↓
Update Cache
```

هنگام تغییر Settings:

```text
Admin
 ↓
Update Settings
 ↓
Database
 ↓
Invalidate Settings Cache
```

تا مقدار جدید در درخواست بعدی استفاده شود.

---

# 18. SEO Infrastructure

از آنجا که DevJourney یک سایت عمومی و دو زبانه است، SEO بخشی از زیرساخت Presentation محسوب می‌شود.

موارد اصلی:

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

Language-aware بودن URLها برای محتوای دو زبانه اهمیت ویژه‌ای دارد.

---

# 19. Performance Principles

Performance در DevJourney بیشتر با بهینه‌سازی‌های متناسب با مسئله دنبال می‌شود، نه با اضافه کردن تکنولوژی‌های بیشتر.

اصول اصلی:

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

قبل از بهینه‌سازی، Bottleneck باید اندازه‌گیری شود.

نباید صرفاً بر اساس حدس، Cache یا Query Optimization به سیستم اضافه شود.

---

# 20. زیرساخت عمداً حذف‌شده

برخی فناوری‌ها عمداً در Infrastructure DevJourney قرار نگرفته‌اند:

```text
RabbitMQ
Kafka
Message Brokers
Microservices Infrastructure
API Gateway
Kubernetes
Elasticsearch
MongoDB
Distributed Cache beyond current needs
```

این حذف به دلیل ضعیف بودن این فناوری‌ها نیست.

دلیل این است که DevJourney در مقیاس فعلی مسئله‌ای ندارد که هزینه و پیچیدگی این ابزارها را توجیه کند.

معماری زیرساخت باید به اندازه مسئله رشد کند.

---

# 21. جمع‌بندی زیرساخت

زیرساخت DevJourney بر پایه این Stack شکل گرفته است:

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

تمام این انتخاب‌ها بر اساس یک اصل مشترک انجام شده‌اند:

```text
Real Problem
     ↓
Appropriate Technology
     ↓
Minimum Necessary Complexity
```

زیرساخت DevJourney قرار نیست به نمایشگاهی از فناوری‌ها تبدیل شود. هدف آن این است که قابلیت‌های واقعی سایت را با ابزارهای استاندارد، قابل نگهداری و متناسب با مقیاس پروژه پیاده‌سازی کند.