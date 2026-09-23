# سیستم چندزبانه و Localization در DevJourney

## 1. مقدمه

DevJourney از ابتدا به‌صورت چندزبانه طراحی می‌شود و در نسخه فعلی از دو زبان فارسی و انگلیسی پشتیبانی می‌کند.

سیستم چندزبانه از دو مفهوم متفاوت تشکیل شده است:

```text
Localization
    ↓
زبان رابط کاربری و Culture

Multilingual Content
    ↓
زبان محتوای Domain
```

این دو مفهوم از یکدیگر جدا هستند.

Localization مشخص می‌کند کاربر با چه زبان و Cultureای با سیستم کار می‌کند، در حالی که Domain مشخص می‌کند هر نوع محتوا چگونه زبان‌دار می‌شود.

---

# 2. زبان‌های پشتیبانی‌شده

زبان‌های فعلی:

```text
fa
en
```

Cultureهای پیش‌فرض:

```text
fa-IR
en-US
```

ساختار سیستم به‌گونه‌ای طراحی می‌شود که اضافه کردن زبان جدید در آینده نیازمند بازطراحی معماری نباشد.

---

# 3. UI Localization

متن‌هایی که بخشی از رابط کاربری هستند توسط ASP.NET Core Localization مدیریت می‌شوند.

نمونه:

```text
صفحه اصلی
پروژه‌ها
دوره‌ها
مقالات
تماس با من
ارسال نظر
ویرایش
حذف
ذخیره
```

این نوع متن‌ها در Database ذخیره نمی‌شوند.

Resourceها در Presentation قرار می‌گیرند:

```text
Resources/
├── SharedResources.fa.resx
└── SharedResources.en.resx
```

و توسط `IStringLocalizer` یا مکانیزم استاندارد Localization مصرف می‌شوند.

---

# 4. Multilingual Domain Content

محتوایی که توسط Admin مدیریت می‌شود، همیشه رفتار یکسانی نسبت به زبان ندارد.

بنابراین Translation به‌صورت Entity-specific طراحی می‌شود و یک Translation Framework عمومی برای تمام Entityها ایجاد نمی‌شود.

## Profile

Profile یک موجودیت واحد است که می‌تواند دو نمایش زبانی داشته باشد:

```text
Profile
├── Common Data
└── ProfileTranslation
    ├── fa
    └── en
```

اطلاعات مشترک مانند:

```text
Avatar
Email
GitHub URL
Social Links
```

ترجمه نمی‌شوند.

مواردی مانند:

```text
Display Name
Headline
About
```

می‌توانند نسخه فارسی و انگلیسی داشته باشند.

---

## Name and Display Name

نام واقعی شخص یک مفهوم هویتی است، اما نحوه نمایش آن می‌تواند بر اساس زبان تغییر کند.

مثلاً:

```text
fa → احسان گلی
en → Ehsan Goli
```

به همین دلیل `DisplayName` می‌تواند بخشی از `ProfileTranslation` باشد.

این مدل از نگهداری همزمان `FirstNameFa`، `FirstNameEn`، `LastNameFa` و `LastNameEn` جلوگیری می‌کند.

---

# 5. Timeline Localization

Timeline یک موجودیت مشترک با محتوای چندزبانه است.

ساختار:

```text
TimelineItem
├── Common Data
│   ├── Date
│   └── Type
└── TimelineItemTranslation
    ├── fa
    │   ├── Title
    │   └── Description
    └── en
        ├── Title
        └── Description
```

تاریخ و metadata مشترک باقی می‌مانند و فقط محتوای متنی ترجمه می‌شود.

---

# 6. Skill Localization

نام تکنولوژی‌ها معمولاً به Translation نیاز ندارند.

مثلاً:

```text
C#
ASP.NET Core
PostgreSQL
Redis
Docker
```

همان مقدار در هر دو زبان باقی می‌مانند.

اما مواردی مانند دسته‌بندی Skill می‌توانند در صورت نیاز ترجمه شوند:

```text
Backend
Databases
DevOps
Architecture
```

بنابراین Localization فقط برای بخش‌هایی اعمال می‌شود که واقعاً دارای تفاوت زبانی هستند.

---

# 7. Portfolio Localization

یک Project یک موجودیت واحد است و اطلاعات فنی آن مستقل از زبان باقی می‌ماند.

مثلاً:

```text
Project
├── Id
├── CoverImage
├── GitHubUrl
├── LiveUrl
├── Technologies
├── StartDate
└── ProjectTranslation
    ├── fa
    │   ├── Title
    │   └── Description
    └── en
        ├── Title
        └── Description
```

مواردی مانند:

```text
GitHub URL
Live URL
Technologies
Images
Dates
```

مشترک هستند.

محتوای نمایشی مانند:

```text
Title
Description
```

ترجمه می‌شود.

---

# 8. Course Localization

Course نیز یک موجودیت واحد با محتوای چندزبانه است:

```text
Course
├── Common Data
│   ├── CoverImage
│   ├── URL
│   └── Metadata
└── CourseTranslation
    ├── fa
    └── en
```

موارد ترجمه‌شونده می‌توانند شامل:

```text
Title
Caption
Description
```

باشند.

---

# 9. Article Language Model

Article از مدل Profile، Project و Course متفاوت است.

هر Article یک محتوای مستقل با یک زبان مشخص است:

```text
Article
├── Id
├── Language
├── Slug
├── Title
├── Summary
├── Content
├── Status
├── PublishedAt
└── ...
```

برای مثال:

```text
Article #1
Language = fa
```

و:

```text
Article #2
Language = en
```

این دو Article مستقل هستند.

یک مقاله فارسی الزاماً ترجمه انگلیسی همان مقاله نیست.

این تصمیم به مدل محتوایی Blog نزدیک‌تر است و از ایجاد ارتباط مصنوعی بین مقالات جلوگیری می‌کند.

---

# 10. Comment Language

Comment به Article مربوط است.

زبان اصلی Comment می‌تواند از زبان Article و Culture فعلی کاربر مشخص شود.

در صورت نیاز به گزارش‌های دقیق‌تر، اطلاعاتی مانند:

```text
Language
Culture
```

می‌توانند همراه Comment ثبت شوند.

Comment ترجمه نمی‌شود.

هر Comment محتوای تولیدشده توسط یک کاربر است و به همان زبان ثبت می‌شود.

---

# 11. Current Culture

Localization Infrastructure مسئول تعیین Culture فعلی Request است.

به‌صورت کلی:

```text
Request
   ↓
Culture Resolution
   ↓
Current Culture
   ↓
Presentation
```

Current Culture می‌تواند مشخص کند:

```text
Language
Date Format
Number Format
Direction
Localized UI
```

مثلاً:

```text
fa-IR
```

منجر به:

```text
lang="fa"
dir="rtl"
```

و:

```text
en-US
```

منجر به:

```text
lang="en"
dir="ltr"
```

می‌شود.

---

# 12. Language Selection Policy

Admin در پنل مدیریت می‌تواند سیاست پیش‌فرض زبان سایت را تعیین کند:

```text
Persian
English
Automatic
```

## Persian

تمام کاربران نسخه فارسی را دریافت می‌کنند مگر اینکه به‌صورت صریح زبان دیگری را انتخاب کرده باشند.

## English

تمام کاربران نسخه انگلیسی را دریافت می‌کنند مگر اینکه به‌صورت صریح زبان دیگری را انتخاب کرده باشند.

## Automatic

زبان برای هر کاربر بر اساس اطلاعات قابل دسترس تعیین می‌شود.

---

# 13. Automatic Language Detection

در حالت Automatic، سیستم از چند Signal استفاده می‌کند.

ترتیب ترجیح:

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

### Explicit User Preference

اگر کاربر خودش زبان را انتخاب کرده باشد، این انتخاب اولویت دارد.

مثلاً:

```text
Detected Language = fa
User selects = en
```

از این لحظه:

```text
Current Language = en
```

و سیستم نباید در هر Request دوباره IP یا Browser Language را بررسی کرده و انتخاب کاربر را تغییر دهد.

---

## Browser Language

`Accept-Language` مرورگر مهم‌ترین Signal خودکار برای زبان است.

مثلاً:

```text
Accept-Language: fa,en;q=0.8
```

به‌طور معمول نشان‌دهنده ترجیح فارسی است.

---

## IP / Geo

IP و اطلاعات جغرافیایی فقط به‌عنوان Signal کمکی استفاده می‌شوند.

برای مثال:

```text
IP → Iran
```

می‌تواند احتمال زبان فارسی را افزایش دهد، اما نباید آن را به‌عنوان حقیقت قطعی در نظر گرفت.

VPN، سفر، Proxy و محل واقعی کاربر می‌توانند باعث اشتباه شوند.

---

## Time Zone

Time Zone نیز به‌عنوان Signal کمکی استفاده می‌شود.

Time Zone به‌تنهایی تعیین‌کننده زبان نیست.

مثلاً:

```text
Asia/Tehran
```

می‌تواند در کنار سایر Signals به تصمیم‌گیری کمک کند، ولی جایگزین Browser Language نمی‌شود.

---

# 14. Manual Language Switching

کاربر همیشه باید بتواند زبان را به‌صورت دستی تغییر دهد.

مثلاً:

```text
فارسی | English
```

انتخاب کاربر باید ذخیره شود تا در بازدیدهای بعدی حفظ شود.

بنابراین:

```text
Automatic Detection
        ↓
Detected = fa
        ↓
User selects English
        ↓
Preference = en
        ↓
Future Requests = en
```

انتخاب صریح کاربر بر تشخیص خودکار اولویت دارد.

---

# 15. URL Strategy

URLهای سایت برای زبان قابل تشخیص هستند.

ساختار پیشنهادی:

```text
/fa
/en
```

مثلاً:

```text
/fa/projects
/en/projects
```

یا:

```text
/fa/articles/clean-architecture
/en/articles/modular-monolith
```

این روش مزایای زیر را دارد:

- زبان URL مشخص است.
- لینک قابل اشتراک‌گذاری است.
- SEO چندزبانه بهتر کنترل می‌شود.
- صفحات زبان‌های مختلف URL مستقل دارند.
- تغییر زبان قابل پیش‌بینی است.

صفحه ریشه:

```text
/
```

می‌تواند بر اساس Language Mode و Language Detection کاربر را به زبان مناسب هدایت کند.

---

# 16. RTL / LTR

جهت صفحه از Culture فعلی تعیین می‌شود.

برای فارسی:

```html
<html lang="fa" dir="rtl">
```

برای انگلیسی:

```html
<html lang="en" dir="ltr">
```

منطق RTL/LTR نباید در بخش‌های مختلف UI به‌صورت دستی و پراکنده پیاده‌سازی شود.

Direction باید از Culture فعلی ناشی شود.

در قالب Frontend، این موضوع با استفاده از CSS Logical Properties (مثل `margin-inline-start`، `padding-inline-end`) به‌جای خواص فیزیکی `left`/`right` پیاده‌سازی شده است؛ به این ترتیب یک Stylesheet واحد هر دو جهت را بدون تکرار پوشش می‌دهد. تعداد کمی از خواص که معادل Logical ندارند از یک متغیر ضریب‌دار جهت‌محور استفاده می‌کنند، و آیکون‌های جهت‌دار به‌جای تعویض در Markup، با CSS برای حالت LTR آینه می‌شوند.

---

# 17. Localization and Caching

Cache باید نسبت به زبان حساس باشد.

برای داده‌های چندزبانه:

```text
homepage:fa
homepage:en
```

یا:

```text
project:{id}:fa
project:{id}:en
```

این کار از نمایش داده فارسی در صفحه انگلیسی و بالعکس جلوگیری می‌کند.

برای Articleهایی که خودشان Language دارند، Cache Key می‌تواند با Language هماهنگ شود:

```text
article:{slug}:fa
article:{slug}:en
```

---

# 18. Fallback Policy

اگر محتوای ترجمه‌شده یک Entity وجود نداشته باشد، رفتار سیستم باید مشخص باشد.

برای Profile، Project، Course و Timeline:

```text
Requested Language
        ↓
Translation Exists?
   ├── Yes → Show Translation
   └── No  → Apply Fallback Policy
```

Fallback می‌تواند بر اساس سیاست تنظیم‌شده سیستم انجام شود.

در نسخه فعلی، Fallback به زبان دیگر نباید بدون تصمیم صریح باعث نمایش محتوای زبان مخالف شود؛ مخصوصاً در صفحات عمومی و SEO-sensitive.

برای Article، چون هر Article یک Language مستقل دارد، Articleای که با زبان URL سازگار نیست نباید به‌عنوان جایگزین نمایش داده شود.

---

# 19. Settings

تنظیمات مرتبط با Localization از پنل مدیریت کنترل می‌شوند.

موارد اصلی:

```text
Language Mode
Default Language
Supported Languages
Automatic Detection
```

نمونه:

```text
Localization Settings

Language Mode:
    ○ Persian
    ○ English
    ○ Automatic

Default Language:
    Persian

Supported Languages:
    [x] Persian
    [x] English
```

این تنظیمات Runtime هستند و در Database ذخیره می‌شوند.

---

# 20. Localization Architecture

مسئولیت‌ها به این شکل تقسیم می‌شوند:

```text
Infrastructure
    ↓
Culture Resolution
    ↓
Current Culture
    ↓
Presentation
    ↓
UI Localization
```

و برای Domain Content:

```text
Current Culture
    ↓
Application
    ↓
Domain Content
    ↓
Translation / Language-specific Content
```

بنابراین Localization Infrastructure مسئول «تشخیص زبان فعلی» است، نه اینکه تمام مدل‌های Domain را خودش مدیریت کند.

---

# 21. Design Principles

سیستم Localization DevJourney بر چند اصل بنا شده است:

### Culture is General

Culture و زبان فعلی یک قابلیت عمومی سیستم است.

### Translation is Domain-specific

نحوه چندزبانه شدن هر Entity بر اساس ماهیت همان Entity تعیین می‌شود.

### User Preference Wins

انتخاب صریح کاربر بالاتر از Automatic Detection است.

### Browser Language is Primary Automatic Signal

`Accept-Language` نسبت به IP و Time Zone اولویت بالاتری دارد.

### IP and Time Zone are Supporting Signals

این اطلاعات فقط برای کمک به Detection اولیه استفاده می‌شوند.

### URLs Should Be Language-aware

زبان باید از URL قابل تشخیص باشد.

### Cache Must Respect Language

نسخه‌های مختلف زبان نباید در Cache یکدیگر را overwrite کنند.

---

# 22. Summary

DevJourney دو سیستم زبانی مستقل ولی هماهنگ دارد:

```text
UI Localization
        +
Domain Content Localization
```

UI توسط ASP.NET Core Localization و Resource Files مدیریت می‌شود.

محتوای Profile، Project، Course و Timeline به‌صورت Entity + Translation مدل می‌شود.

مقالات به‌صورت Content مستقل با Language مشخص مدل می‌شوند.

زبان کاربر می‌تواند توسط Admin روی Persian، English یا Automatic تنظیم شود.

در Automatic Mode، سیستم از User Preference، URL، Cookie، Browser Language و سپس Signals کمکی مانند IP و Time Zone استفاده می‌کند.

کاربر همیشه امکان تغییر زبان را دارد و انتخاب دستی او بر Detection خودکار اولویت دارد.

این طراحی باعث می‌شود چندزبانه بودن DevJourney یک قابلیت زیرساختی منسجم باشد، بدون اینکه تمام Domain را به یک Translation Framework عمومی و پیچیده وابسته کند.