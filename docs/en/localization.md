# DevJourney Localization

## 1. Introduction

DevJourney is designed as a multilingual platform from the beginning and currently supports two languages:

```text id="w5q4m2"
fa
en
```

The multilingual system is based on two distinct concepts:

```text id="c8z2p1"
Localization
    ↓
UI language and Culture

Multilingual Content
    ↓
Domain content language
```

These concepts are intentionally kept separate.

Localization determines how the user interacts with the system in terms of language and culture, while the Domain determines how each type of content is represented across languages.

---

# 2. Supported Languages

The current supported languages are:

```text id="r2x6n8"
fa
en
```

Default cultures are:

```text id="k9c3v7"
fa-IR
en-US
```

The localization architecture is designed so that adding another language in the future does not require redesigning the system.

---

# 3. UI Localization

Text that belongs to the user interface is handled through ASP.NET Core Localization.

Examples include:

```text id="x4v8j2"
Home
Projects
Courses
Articles
Contact
Submit Comment
Edit
Delete
Save
```

These texts are not stored in the database.

UI resources are stored in resource files:

```text id="a7m5q1"
Resources/
├── SharedResources.fa.resx
└── SharedResources.en.resx
```

They are consumed through `IStringLocalizer` or the standard ASP.NET Core localization mechanisms.

---

# 4. Multilingual Domain Content

Administrator-managed content does not necessarily have the same language model for every entity.

Therefore, translation is modeled on an entity-specific basis rather than through a single generic translation framework for the entire Domain.

## Profile

Profile represents a single conceptual entity with multiple localized representations:

```text id="c3y7k9"
Profile
├── Common Data
└── ProfileTranslation
    ├── fa
    └── en
```

Shared information such as:

```text id="q8m2v6"
Avatar
Email
GitHub URL
Social Links
```

is not translated.

Localized information such as:

```text id="p1n5x7"
Display Name
Headline
About
```

may have Persian and English representations.

---

# 5. Name and Display Name

A person's real name is an identity concept rather than ordinary translated content, but its presentation may differ by language.

For example:

```text id="h6r2m9"
fa → احسان گلی
en → Ehsan Goli
```

For this reason, `DisplayName` may belong to `ProfileTranslation`.

This avoids maintaining separate `FirstNameFa`, `FirstNameEn`, `LastNameFa`, and `LastNameEn` properties.

---

# 6. Timeline Localization

Timeline entries represent the same conceptual entry with localized textual content.

The structure is:

```text id="t5k8q3"
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

Dates and non-language metadata remain shared while textual content is localized.

---

# 7. Skill Localization

Technology names generally do not require translation.

For example:

```text id="j3m7w1"
C#
ASP.NET Core
PostgreSQL
Redis
Docker
```

remain the same in both languages.

However, categories such as:

```text id="p9x4k6"
Backend
Databases
DevOps
Architecture
```

may be localized when necessary.

Localization is therefore applied only where an actual language difference exists.

---

# 8. Portfolio Localization

A Project is a single entity whose technical information remains language-independent.

For example:

```text id="r6q2v8"
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

Shared information includes:

```text id="b8z5n2"
GitHub URL
Live URL
Technologies
Images
Dates
```

Localized information includes:

```text id="m4y7q1"
Title
Description
```

---

# 9. Course Localization

Courses follow the same model:

```text id="d2v6k9"
Course
├── Common Data
│   ├── CoverImage
│   ├── URL
│   └── Metadata
└── CourseTranslation
    ├── fa
    └── en
```

Localized content may include:

```text id="q5w8r3"
Title
Caption
Description
```

---

# 10. Article Language Model

Articles use a different model from Profile, Project, and Course.

Each Article is an independent piece of content with a single language:

```text id="n8c4m2"
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

For example:

```text id="v3k7p1"
Article #1
Language = fa
```

and:

```text id="h9m2x6"
Article #2
Language = en
```

These are independent articles.

A Persian article is not necessarily an English translation of the same article.

This model reflects the actual content structure of the DevJourney blog and avoids creating artificial relationships between independent articles.

---

# 11. Comment Language

Comments belong to an Article.

The primary language of a Comment can be inferred from the Article language and the user's current Culture.

When useful for analytics or moderation, additional information such as:

```text id="s4q7m1"
Language
Culture
```

may also be stored with the Comment.

Comments are not translated.

Each Comment is user-generated content and remains in its original language.

---

# 12. Current Culture

The Localization Infrastructure is responsible for resolving the current Culture of the request.

The general flow is:

```text id="x8k2p5"
Request
   ↓
Culture Resolution
   ↓
Current Culture
   ↓
Presentation
```

Current Culture determines information such as:

```text id="m7q3v9"
Language
Date Format
Number Format
Direction
Localized UI
```

For example:

```text id="q1n6x4"
fa-IR
```

results in:

```text id="b8m3k7"
lang="fa"
dir="rtl"
```

while:

```text id="c5r9w2"
en-US
```

results in:

```text id="z6p4m8"
lang="en"
dir="ltr"
```

---

# 13. Language Selection Policy

The administrator can configure the site's default language policy:

```text id="w2h7k4"
Persian
English
Automatic
```

## Persian

All users receive the Persian version unless they explicitly select another language.

## English

All users receive the English version unless they explicitly select another language.

## Automatic

The language is resolved based on available user and request information.

---

# 14. Automatic Language Detection

In Automatic mode, the system uses multiple signals.

The preferred order is:

```text id="f6m2q9"
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

If the user has explicitly selected a language, that choice takes priority.

For example:

```text id="k4r8p2"
Detected Language = fa
User selects = en
```

From that point:

```text id="y7m3q5"
Current Language = en
```

The system must not repeatedly override this choice based on IP or browser detection.

---

## Browser Language

The browser's `Accept-Language` header is the primary automatic signal.

For example:

```text id="v8q3m1"
Accept-Language: fa,en;q=0.8
```

generally indicates a preference for Persian.

---

## IP / Geo

IP-based geographic information is treated as a supporting signal.

For example:

```text id="n5k2r7"
IP → Iran
```

may increase the likelihood of choosing Persian.

However, it must not be treated as an authoritative indication of language because VPNs, proxies, travel, and other factors can make geographic detection inaccurate.

---

## Time Zone

Time Zone is also treated as a supporting signal.

Time Zone is not sufficient by itself to determine a user's language.

For example:

```text id="m8q4x1"
Asia/Tehran
```

may contribute to the decision when combined with other signals, but it does not override browser language or explicit user preference.

---

# 15. Manual Language Switching

Users can always manually switch between supported languages.

For example:

```text id="r7p2c9"
فارسی | English
```

The user's selection is persisted so that it can be respected during future visits.

The flow is:

```text id="w5n8q3"
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

Explicit user choice always takes priority over automatic detection.

---

# 16. URL Strategy

Public URLs are language-aware.

The recommended structure is:

```text id="k1m5r8"
/fa
/en
```

Examples:

```text id="c7q3p9"
/fa/projects
/en/projects
```

and:

```text id="x4v8m2"
/fa/articles/clean-architecture
/en/articles/modular-architecture
```

This provides:

- Explicit language identification in the URL
- Shareable language-specific links
- Better control over multilingual SEO
- Independent URLs for localized pages
- Predictable language switching

The root URL:

```text id="n2z6w4"
/
```

may redirect to the appropriate language based on the configured language mode and detected user language.

---

# 17. RTL / LTR

Page direction is determined by the current Culture.

For Persian:

```html id="4j9qv2"
<html lang="fa" dir="rtl">
```

For English:

```html id="r6m3x8"
<html lang="en" dir="ltr">
```

RTL/LTR logic should not be manually duplicated across individual UI components.

Direction should derive from the resolved Culture.

---

# 18. Localization and Caching

Cache keys must be language-aware for localized content.

Examples:

```text id="p5w8k2"
homepage:fa
homepage:en
```

and:

```text id="m7q3r1"
project:{id}:fa
project:{id}:en
```

This prevents one language version from overwriting another.

For Articles, which have their own language, cache keys can also include the language:

```text id="x9n4v6"
article:{slug}:fa
article:{slug}:en
```

---

# 19. Fallback Policy

The system must define explicit fallback behavior when localized content is missing.

For Profile, Project, Course, and Timeline:

```text id="t2q7m5"
Requested Language
        ↓
Translation Exists?
   ├── Yes → Show Translation
   └── No  → Apply Fallback Policy
```

Fallback behavior must be deliberate rather than silently displaying content in another language.

For the current version, missing translations should not automatically expose content in the other language, especially on public SEO-sensitive pages.

For Articles, each Article has its own language, so an Article whose language does not match the requested language should not be presented as a substitute.

---

# 20. Settings

Localization-related settings are managed through the administration panel.

Primary settings include:

```text id="z8m2p4"
Language Mode
Default Language
Supported Languages
Automatic Detection
```

Example:

```text id="v5q7k1"
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

These are runtime settings and are stored in the database.

---

# 21. Localization Architecture

Responsibilities are divided as follows:

```text id="y3m8q2"
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

For Domain content:

```text id="p6r1v9"
Current Culture
    ↓
Application
    ↓
Domain Content
    ↓
Translation / Language-specific Content
```

Localization Infrastructure is therefore responsible for resolving the current language and Culture, not for controlling every Domain translation model.

---

# 22. Design Principles

The DevJourney localization system follows several principles.

### Culture is General

Current Culture and language are system-wide capabilities.

### Translation is Domain-specific

How an entity supports multiple languages is determined by that entity's Domain requirements.

### User Preference Wins

Explicit user selection always has higher priority than automatic detection.

### Browser Language is the Primary Automatic Signal

`Accept-Language` has higher priority than IP and Time Zone for automatic language detection.

### IP and Time Zone are Supporting Signals

They may help with initial detection but are not authoritative language sources.

### URLs Should Be Language-aware

Language should be identifiable from public URLs.

### Cache Must Respect Language

Different localized representations must never overwrite each other in cache storage.

---

# 23. Summary

DevJourney uses two coordinated but distinct localization systems:

```text id="c4n7m2"
UI Localization
        +
Domain Content Localization
```

The UI is localized using ASP.NET Core Localization and resource files.

Profile, Project, Course, and Timeline content use an Entity + Translation model where a single conceptual entity can have multiple localized representations.

Articles are modeled as independent language-specific content.

The administrator can configure the site's language policy as Persian, English, or Automatic.

In Automatic mode, the system considers explicit user preference, URL, cookie, browser language, and then supporting signals such as IP and Time Zone.

Users can always switch languages manually, and explicit user choice takes priority over automatic detection.

This design makes multilingual support a coherent platform capability without forcing the entire Domain to depend on a generic and unnecessarily complex translation framework.