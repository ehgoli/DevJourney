# DevJourney

> یک پلتفرم شخصی برای معرفی حرفه‌ای، مهارت‌ها، مسیر کاری، پروژه‌ها، دوره‌ها و مقالات فنی که با ASP.NET Core طراحی و پیاده‌سازی شده است.

[![وضعیت پروژه](https://img.shields.io/badge/status-in%20development-orange)](#وضعیت-پروژه)
[![.NET](https://img.shields.io/badge/.NET-10-512BD4)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Razor%20Pages-512BD4)](https://learn.microsoft.com/aspnet/core/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-18-4169E1)](https://www.postgresql.org/)
[![Redis](https://img.shields.io/badge/Redis-cache-DC382D)](https://redis.io/)

---

## معرفی پروژه

**DevJourney** یک وب‌سایت شخصی و پلتفرم محتوایی چندزبانه است که با هدف ارائه ساختاریافته هویت حرفه‌ای، مهارت‌ها، مسیر فعالیت، پروژه‌ها، دوره‌های آموزشی و مقالات فنی طراحی شده است.

این پروژه علاوه بر وب‌سایت عمومی، شامل یک پنل مدیریت برای مدیریت محتوا، نظرات، تنظیمات، تحلیل رفتار کاربران، قابلیت‌های هوش مصنوعی و سایر بخش‌های مدیریتی است.

DevJourney به‌صورت **Layered Monolithic Architecture** طراحی شده و از پیچیدگی‌های معماری توزیع‌شده‌ای که در مقیاس فعلی پروژه توجیه ندارند، عمداً استفاده نمی‌کند.

هدف اصلی پروژه افزایش تعداد تکنولوژی‌ها یا Design Patternها نیست؛ بلکه نمایش نحوه انتخاب و پیاده‌سازی راهکارهای فنی و معماری متناسب با یک مسئله واقعی است.

---

# وضعیت پروژه

**در حال توسعه**

پروژه به‌صورت مرحله‌ای توسعه داده می‌شود و معماری، زیرساخت، مستندات و قابلیت‌های مختلف همزمان با پیشرفت واقعی پروژه تکمیل می‌شوند.

---

# قابلیت‌ها

## وب‌سایت عمومی

- پروفایل شخصی و معرفی حرفه‌ای
- مهارت‌ها و میزان تسلط
- Timeline مسیر حرفه‌ای
- دوره‌های آموزشی
- نمونه‌کارها و پروژه‌ها
- لینک پروژه‌های GitHub
- مقالات فنی
- نظرات مقالات
- فرم تماس
- شبکه‌های اجتماعی
- رابط کاربری Responsive
- پشتیبانی از زبان فارسی و انگلیسی

## مدیریت محتوا

پنل مدیریت امکان مدیریت موارد زیر را فراهم می‌کند:

- پروفایل
- مهارت‌ها
- Timeline
- پروژه‌ها
- دوره‌ها
- مقالات
- نظرات
- Analytics
- Localization
- تنظیمات سایت
- قابلیت‌های هوش مصنوعی مرتبط با محتوا

## قابلیت‌های هوش مصنوعی

هوش مصنوعی به‌عنوان یک Integration زیرساختی استفاده می‌شود و بخشی از Domain اصلی سیستم نیست.

قابلیت‌های در نظر گرفته‌شده:

- بررسی و Moderation نظرات
- بهبود متن مقاله
- تولید خلاصه
- پیشنهاد عنوان
- پیشنهاد Tag
- کمک به بهینه‌سازی SEO

## Analytics

سیستم Analytics رفتار کاربران را ثبت و تجمیع می‌کند.

نمونه اطلاعات:

- تعداد بازدیدها
- کاربران یکتا
- مشاهده مقالات
- مدت زمان حضور
- زمان مطالعه مقاله
- میزان پیشرفت مطالعه
- عمق Scroll
- تکمیل مطالعه مقاله
- بازدید پروژه‌ها
- تعامل با دوره‌ها
- منابع ورودی
- توزیع کاربران بر اساس زبان

پنل مدیریت این داده‌ها را به‌صورت Dashboard، نمودار و گزارش نمایش می‌دهد.

## پردازش‌های Background

برای کارهایی که نیاز به اجرای پس‌زمینه یا زمان‌بندی دارند از Background Processing استفاده می‌شود.

نمونه‌ها:

- انتشار زمان‌بندی‌شده مقاله
- Moderation نظرات با AI
- تجمیع داده‌های Analytics
- سایر عملیات تأخیری یا پس‌زمینه

برای Jobهای زمان‌بندی‌شده و پایدار از Hangfire و برای کارهای ساده‌تر از مکانیزم‌های Background داخلی ASP.NET Core استفاده می‌شود.

---

# معماری

DevJourney از یک **Layered Monolithic Architecture** با تکیه بر اصول Clean Architecture استفاده می‌کند.

```text id="s7nj9q"
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

چهار لایه اصلی:

```text id="4p5m8x"
Presentation
Application
Domain
Infrastructure
```

### Presentation

مسئول:

- Razor Pages
- وب‌سایت عمومی
- پنل مدیریت
- Routing
- Model Binding
- Localization رابط کاربری
- مرزهای Authentication و Authorization

### Application

مسئول:

- Application Services
- DTOها
- Validation
- Mapping
- Abstractionهای Application
- هماهنگی بین Domain و Infrastructure

### Domain

مسئول:

- Entityها
- Value Objectها
- Business Ruleها
- Factoryها
- Domain Serviceها
- Domain Eventها
- Exceptionهای دامنه

### Infrastructure

مسئول:

- دسترسی به Database
- EF Core
- Dapper
- Redis
- HybridCache
- AI Integration
- Background Jobs
- سرویس‌های خارجی
- Localization Infrastructure
- Logging
- Observability
- File Storage

جزئیات کامل معماری در:

[مستند معماری](docs/fa/architecture.md)

---

# تکنولوژی‌های اصلی

| حوزه | تکنولوژی | کاربرد |
|---|---|---|
| Web | ASP.NET Core | پلتفرم اصلی برنامه |
| UI | Razor Pages | رابط کاربری Server-rendered |
| Database | PostgreSQL | پایگاه داده اصلی |
| ORM | EF Core | Persistence و عملیات Write |
| SQL | Dapper | Queryهای تخصصی و Analytics |
| Cache | Redis | Distributed Cache |
| Cache API | HybridCache | ترکیب Local و Distributed Cache |
| Background Jobs | Hangfire | Jobهای زمان‌بندی‌شده و پایدار |
| AI | External AI Provider | Moderation و کمک به تولید محتوا |
| Localization | ASP.NET Core Localization | مدیریت Culture و زبان UI |
| Logging | Serilog | Structured Logging |
| Observability | OpenTelemetry | Metrics و Tracing |
| Validation | Application Validation | اعتبارسنجی ورودی |
| Testing | xUnit | تست خودکار |
| Source Control | Git / GitHub | مدیریت Source Code |

این پروژه عمداً از تکنولوژی‌هایی مانند Message Broker، Microservices، Kubernetes، MongoDB، Elasticsearch و API Gateway استفاده نمی‌کند؛ زیرا در مقیاس فعلی مسئله‌ای که هزینه این پیچیدگی را توجیه کند وجود ندارد.

---

# پشتیبانی از چند زبان

DevJourney از دو زبان زیر پشتیبانی می‌کند:

```text id="22zh9p"
فارسی
English
```

سیستم زبان دو مفهوم اصلی را از هم جدا می‌کند:

```text id="ts6pdj"
UI Localization
        +
Domain Content Localization
```

### Localization رابط کاربری

متن‌های ثابت رابط کاربری با ASP.NET Core Localization و Resource Fileها مدیریت می‌شوند.

### Localization محتوا

Profile، Project، Course و Timeline می‌توانند نسخه فارسی و انگلیسی داشته باشند.

### زبان مقاله

مقالات به‌صورت محتوای مستقل و زبان‌محور مدل می‌شوند. یک مقاله فارسی و یک مقاله انگلیسی می‌توانند دو رکورد مستقل باشند.

### تشخیص زبان

ادمین می‌تواند سیاست زبان سایت را روی یکی از این حالت‌ها قرار دهد:

```text id="6s8xkc"
فارسی
English
Automatic
```

در حالت Automatic از Preference کاربر، URL، Cookie، زبان Browser و سیگنال‌های کمکی مانند IP و Time Zone استفاده می‌شود.

کاربر همیشه می‌تواند زبان دیگر را به‌صورت دستی انتخاب کند.

جزئیات کامل در:

[مستند Localization](docs/fa/localization.md)

---

# سیستم کشینگ

Caching به‌صورت انتخابی و نه سراسری اعمال می‌شود.

داده‌های مناسب برای Cache شامل:

```text id="5ta4so"
Homepage
Profile
Projects
Courses
Published Articles
Popular Content
Analytics Summary
Runtime Settings
```

ساختار کلی:

```text id="f0wm4t"
Application
    ↓
HybridCache
    ├── Memory Cache
    └── Redis
```

Cache Keyهای داده‌های چندزبانه نیز وابسته به زبان هستند:

```text id="u2z9n3"
homepage:fa
homepage:en

project:{id}:fa
project:{id}:en
```

در صورت تغییر داده، Cache مربوطه Invalidate می‌شود.

جزئیات در:

[مستند زیرساخت](docs/fa/infrastructure.md)

---

# یکپارچه‌سازی هوش مصنوعی

Integrationهای هوش مصنوعی از طریق abstractionهای Application و پیاده‌سازی‌های Infrastructure انجام می‌شوند.

برای مثال:

```text id="i14ey0"
Application
    ↓
IAiModerationService
    ↓
AI Adapter
    ↓
External AI Provider
```

به این ترتیب Domain و Application به یک Provider خاص وابسته نمی‌شوند.

قابلیت‌های در نظر گرفته‌شده:

- Moderation نظرات
- بهبود مقاله
- تولید خلاصه
- پیشنهاد عنوان
- پیشنهاد Tag
- کمک به SEO

خروجی AI نقش پیشنهاد و کمک را دارد و کنترل نهایی محتوای منتشرشده همچنان در اختیار نویسنده یا Administrator است.

---

# Analytics

سیستم Analytics رویدادهای مربوط به رفتار کاربران را ثبت و سپس آن‌ها را به Metricهای قابل استفاده تبدیل می‌کند.

جریان کلی:

```text id="1ds7a8"
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

نمونه سؤالاتی که Dashboard می‌تواند پاسخ دهد:

- چه تعداد کاربر از سایت بازدید کرده‌اند؟
- کدام مقاله بیشترین بازدید را داشته است؟
- کاربران چقدر در یک صفحه باقی می‌مانند؟
- کاربران چه مقدار از یک مقاله را مطالعه می‌کنند؟
- کدام پروژه بیشترین توجه را دریافت کرده است؟
- ترافیک فارسی و انگلیسی چه تفاوتی دارد؟
- کاربران از چه منابعی وارد سایت شده‌اند؟

---

# اصول طراحی و Design Patternها

اصل اساسی DevJourney این است:

> **ساده‌ترین راهکاری را انتخاب کن که مسئله واقعی را حل کند و فقط زمانی abstraction یا Design Pattern جدید اضافه کن که ارزش معماری واقعی ایجاد کند.**

Patternهای احتمالی مورد استفاده شامل:

- Factory Method
- Repository
- Unit of Work
- Strategy
- Adapter
- Decorator
- State

Patternها صرفاً برای نمایش مهارت یا افزایش تعداد Patternهای پروژه استفاده نمی‌شوند.

همچنین پروژه عمداً از موارد زیر اجتناب می‌کند:

- Microservices
- Message Broker
- Mediator / MediatR
- CQRS اجباری
- Generic Repository
- Unit of Work بیش‌ازحد انتزاعی
- چند Database بدون نیاز واقعی

دلیل این تصمیم‌ها در مستند معماری توضیح داده شده است:

[مستند معماری و تصمیمات طراحی](docs/fa/architecture.md)

---

# ساختار Repository

```text id="f0jsh5"
DevJourney/
│
├── src/
│   ├── DevJourney.Presentation/
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
├── docs/fa/
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

# قالب Frontend

در Repository، قالب Frontend نیز به‌صورت مستقل نگهداری می‌شود:

```text id="q7k4r2"
frontend/
└── public/
```

هدف از جدا نگه داشتن Template این است که لایه بصری پروژه بدون وابستگی به معماری Backend قابل بررسی، استفاده و توسعه باشد.

بنابراین بازدیدکننده GitHub می‌تواند Template HTML/CSS/JS را جداگانه بررسی کند و در صورت نیاز وارد کد ASP.NET Core شود.

---

# تصاویر پروژه

تصاویر رابط کاربری در مسیر زیر قرار می‌گیرند:

```text id="v5p8j3"
screenshots/
├── home-fa.png
├── home-en.png
├── article.png
├── admin-dashboard.png
├── analytics.png
├── settings.png
└── comments-moderation.png
```

> تصاویر همزمان با تکمیل بخش‌های مختلف پروژه به‌روزرسانی خواهند شد.

---

# مستندات

مستندات پروژه عمداً محدود و متمرکز نگه داشته شده‌اند.

### معماری

در این سند موارد زیر بررسی می‌شوند:

- معماری کلی
- مسئولیت لایه‌ها
- Dependency Rules
- Application Services
- Design Patterns
- Architectural Decisions
- Trade-offهای مهم

[مطالعه مستند معماری](docs/fa/architecture.md)

### زیرساخت

در این سند تکنولوژی‌های زیرساختی، نقش آن‌ها و دلیل انتخاب هرکدام بررسی می‌شود.

[مطالعه مستند زیرساخت](docs/fa/infrastructure.md)

### Localization

در این سند سیستم چندزبانه، Culture، تشخیص زبان، URL، RTL/LTR و Language-aware Caching توضیح داده می‌شود.

[مطالعه مستند Localization](docs/fa/localization.md)

---

# توسعه پروژه

DevJourney در حال توسعه است.

راهنمای توسعه پروژه عمداً تا زمانی که نیازهای عملی آن شکل نگرفته‌اند، بیش از حد پیچیده نمی‌شود. تنظیمات موردنیاز، Database، Redis، AI Provider و سایر پیش‌نیازهای فنی همزمان با پیاده‌سازی زیرساخت مربوطه مستند خواهند شد.

برای وضعیت فعلی توسعه، به Source Code و Configuration موجود در Repository مراجعه کنید.

---

# مجوز

این پروژه با هدف ارائه یک نمونه عملی از توسعه نرم‌افزار و نمایش معماری و مهندسی Backend در GitHub منتشر می‌شود.

License نهایی پروژه پیش از اولین Release پایدار مشخص خواهد شد.

---

# توسعه‌دهنده

**احسان گلی**

توسعه‌دهنده Backend و Full-Stack

حوزه‌های اصلی:

```text id="4v1hgo"
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

# هدف پروژه

DevJourney صرفاً یک سایت رزومه شخصی نیست.

هدف آن ساخت یک نمونه واقعی از یک Web Application قابل نگهداری است که در آن تصمیم‌های معماری و فنی بر اساس نیاز واقعی سیستم، سادگی، توسعه‌پذیری و کیفیت مهندسی گرفته می‌شوند.

تمرکز اصلی پروژه:

```text id="b8t0k5"
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

Source Code، قالب Frontend، مستندات، تصمیم‌های معماری و روند توسعه همگی بخشی از DevJourney هستند.