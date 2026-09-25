# Developer Guide

A reference for extending this template: how the CSS is organized, how the
RTL/LTR and dark mode systems work, and step-by-step instructions for adding
pages, sections, and components without breaking the existing design.

Read this once before you touch the code. Most "where do I put this?"
questions are answered in the first three sections.

## Contents

1. [Project structure](#1-project-structure)
2. [Pages at a glance](#2-pages-at-a-glance)
3. [CSS architecture & load order](#3-css-architecture--load-order)
4. [The typography scale (`--fs-*` variables)](#4-the-typography-scale---fs--variables)
5. [Color system & dark mode](#5-color-system--dark-mode)
6. [Bilingual RTL/LTR system](#6-bilingual-rtlltr-system)
7. [Reusable components](#7-reusable-components)
8. [JavaScript behaviors](#8-javascript-behaviors)
9. [Admin panel](#9-admin-panel)
9a. [Auth pages](#9a-auth-pages)
10. [How to add a new page](#10-how-to-add-a-new-page)
11. [How to add a new section to an existing page](#11-how-to-add-a-new-section-to-an-existing-page)
12. [How to add a new reusable component](#12-how-to-add-a-new-reusable-component)
13. [Known dead code](#13-known-dead-code)
14. [Possible future backend migration](#14-possible-future-backend-migration)
15. [Checklist before committing](#15-checklist-before-committing)

---

## 1. Project structure

```
Resume Html/
├── Resume.html              Homepage (FA/RTL)         — its own hero/header
├── Resume.en.html           Homepage (EN/LTR)
├── article.html             Single blog post (FA)
├── article.en.html          Single blog post (EN)
├── articles-ajax.html       Blog archive + live search (FA)
├── articles-ajax.en.html    Blog archive + live search (EN)
├── ProjectDetails.html      Single project detail (FA)
├── ProjectDetails.en.html   Single project detail (EN)
├── CourseDetails.html       Single course detail (FA)
├── CourseDetails.en.html    Single course detail (EN)
├── courses.html             All courses / archive (FA)
├── courses.en.html          All courses / archive (EN)
├── 404.html                 Not-found page (FA)
├── 404.en.html              Not-found page (EN)
├── 403.html                 Access-denied page (FA)
├── 403.en.html              Access-denied page (EN)
├── Login.html               Sign in — phone + password (FA/RTL)
├── Login.en.html            Sign in (EN/LTR)
├── Signup.html              Create account (FA/RTL)
├── Signup.en.html           Create account (EN/LTR)
├── ForgotPassword.html      Password recovery — phone → code → new password (FA/RTL)
├── ForgotPassword.en.html   Password recovery (EN/LTR)
├── AdminLayout.html         Admin panel shell + sample page (FA) — not a real page
├── AdminLayout.en.html      Admin panel shell + sample page (EN) — not a real page
├── layout.html              Starting point for new public pages (FA) — not a real page
├── layout.en.html           Starting point for new public pages (EN) — not a real page
├── datas.json               Sample data consumed by the live article search
├── DEVELOPER-GUIDE.md        This file
└── assets/
    ├── vendor/
    │   ├── bootstrap/        Bootstrap 5 (+ RTL build) and Bootstrap Icons — vendor, don't edit
    │   ├── video-js/         video.js player (JS + CSS) — vendor, don't edit
    │   └── tsparticles/      tsParticles engine + slim bundle — vendor, don't edit
    ├── fonts/                IRANYekanX (FA) + Inter (EN) + the stylesheet that switches between them
    ├── images/               Avatar, backgrounds, certificate images, etc.
    ├── downloads/            Placeholder files for lesson "download attachment" buttons
    ├── js/                   Template's own scripts (see §8) — nothing vendor here anymore
    └── css/
        ├── layout.css                    Root variables, reset, shared header & footer
        ├── components.css                Reusable cards & form elements (shared across pages)
        ├── dark-mode.css                  Dark theme overrides — always loaded last
        ├── resume.css                     Homepage-only styles
        ├── article.css                    Article-page-only styles
        ├── project-details.css            Project-detail-page-only styles
        ├── course-details.css             Course-detail-page-only styles
        ├── courses-archive.css            Courses-archive-page-only styles
        ├── articles-updated-ajax-v2.css   Article-archive-page-only styles
        ├── error-page.css                 Shared by 404 + 403 (and any future error page)
        ├── admin-layout.css               Admin panel shell styles
        ├── auth.css                       Shared by Login / Signup / Forgot Password
        ├── particles-bg.css               Standalone particle-background component
        └── video-player.css               video.js re-skin — see §8
```

Everything under `assets/vendor/` is vendor code (Bootstrap + Bootstrap
Icons, video.js, tsParticles) — don't hand-edit it. Everything else is
this template's own code.

## 2. Pages at a glance

| Page | Purpose | Own CSS file | Notes |
|---|---|---|---|
| `Resume.html` / `.en.html` | Homepage | `resume.css` | Hero, skills, timeline, courses, GitHub projects, portfolio carousel, certificates, articles preview, contact form. Has its own header (no shared `site-header`). |
| `article.html` / `.en.html` | Single blog post | `article.css` | Post body, comments, sidebar (author + related + tags). |
| `articles-ajax.html` / `.en.html` | Blog archive | `articles-updated-ajax-v2.css` | Search, filters, sort, pagination. Static sample output — see §8. |
| `ProjectDetails.html` / `.en.html` | Single project | `project-details.css` | Gallery, tech stack, contributors, sidebar. |
| `CourseDetails.html` / `.en.html` | Single course | `course-details.css` | Highlights checklist, curriculum accordion, prerequisites, sidebar. Linked from the "Clean Architecture" course card on the homepage. |
| `courses.html` / `.en.html` | All courses (archive) | `courses-archive.css` | Client-side level filter (no search/pagination — only a handful of courses exist). Reuses `.course-card` from `components.css` as-is. Linked from the homepage's "View all courses". |
| `404.html` / `.en.html` | Not-found page | `error-page.css` | Terminal-style illustration (inline SVG), no `components.css` needed. Named `404.html` on purpose — most static hosts (GitHub Pages, Netlify, etc.) auto-serve a file with this exact name for any unmatched URL, no server config required. |
| `403.html` / `.en.html` | Access-denied page | `error-page.css` (shared with 404) | Same terminal illustration family, showing a 403 response instead, plus a lock icon. |
| `Login.html` / `.en.html` | Sign in | `auth.css` | Phone + password, no site header/footer — a centered card on the default background with the particle layer. See §9a. |
| `Signup.html` / `.en.html` | Create account | `auth.css` | Full name, phone, password, terms checkbox. Same card/layout as Login. |
| `ForgotPassword.html` / `.en.html` | Password recovery | `auth.css` | Phone → verification code → new password, as a 3-step wizard inside one card (plus a final "done" panel). See §9a. |
| `AdminLayout.html` / `.en.html` | **Not a real page** | `admin-layout.css` | Admin panel shell + sample page. See §9. |
| `layout.html` / `.en.html` | **Not a real page** | — | Starting point for new pages. See §10. |

Adding another error page later (500, 401, a maintenance page, etc.)? Copy
`403.html`/`.en.html` rather than starting fresh — keep `error-page.css` as
the shared file, reuse the `.error-*` classes as they are, and only swap
out the inline SVG content and the eyebrow/title/description text. That's
what keeps error pages reading as one consistent family instead of each
looking like a one-off.

Every real page follows the same `<head>` pattern:

```html
<script src="assets/js/theme-init.js"></script>      <!-- must be first, blocking -->
<link href="assets/vendor/bootstrap/css/bootstrap[.rtl].css" rel="stylesheet">
<link href="assets/vendor/bootstrap/bootstrap-icons/bootstrap-icons.css" rel="stylesheet">
<link href="assets/css/layout.css" rel="stylesheet">
<link href="assets/css/components.css" rel="stylesheet">
<link href="assets/fonts/style.css" rel="stylesheet">
<link href="assets/css/<page-specific>.css" rel="stylesheet">
<link href="assets/css/particles-bg.css" rel="stylesheet">   <!-- only on pages that use it -->
<link href="assets/css/dark-mode.css" rel="stylesheet">      <!-- always last -->
```

The FA pages (`dir="rtl" lang="fa"`) and EN pages (`dir="ltr" lang="en"`) are
**not** maintained as independent designs — they're the same HTML structure
and the same classes, just with translated text, a different Bootstrap build,
and a different `dir`/`lang`. See §6 for how the CSS makes that work with no
page-specific overrides.

## 3. CSS architecture & load order

Four layers, always loaded in this order:

1. **`layout.css`** — the foundation. Color variables and the typography
   scale (`:root`), the browser reset, `body`/`container` rules, the base
   `.card-custom` surface, and the shared `site-header` / mobile offcanvas
   menu / `site-footer` (used by every page except the homepage, which has
   its own hero).
2. **`components.css`** — building blocks used on *more than one* page:
   article card, project card, course card, GitHub project card, sidebar
   widget, form fields, breadcrumb. If you're about to copy-paste a card from
   one page's CSS into another page's CSS, it probably belongs here instead.
3. **The page's own CSS file** — layout and styling specific to that one
   page. When a shared component needs a small per-page tweak (e.g. a
   slightly different padding), that override goes here, scoped as narrowly
   as possible, *not* into `components.css`.
4. **`dark-mode.css`** — always loaded last so its overrides win. See §5.

**Rule of thumb:** if a class name or visual pattern would make sense reused
on a different page, it belongs in `components.css`. If it only ever applies
to one page's layout, it belongs in that page's own CSS file.

## 4. The typography scale (`--fs-*` variables)

All font sizes that change across breakpoints are driven by `--fs-*` custom
properties defined once in `layout.css`'s `:root`, almost all using
`clamp(min, calc(...), max)` so size changes smoothly with viewport width
instead of jumping at breakpoints:

```css
--fs-project-title: clamp(19px, calc(15.571px + 0.4464vw), 20px);
```

```css
.project-title {
    font-size: var(--fs-project-title);
}
```

Variables are grouped by the file that consumes them (header/footer, shared
components, article/project-detail pages, the article archive, the
homepage), with a comment above each group in `layout.css`. Two variables
that don't scale monotonically (they briefly grow again at a middle
breakpoint) can't be expressed as a single `clamp()`, so they're kept as a
discrete base value plus `@media { :root { --fs-x: ...; } }` overrides
instead — `--fs-article-title` and `--fs-contact-title` are the only two
that work this way, and they're the *only* place font-size should ever be
redefined inside a media query.

**When you add a new element that needs responsive text:**

1. Check whether an existing `--fs-*` variable already fits (several are
   intentionally shared — e.g. the article, project-detail, and
   course-detail pages all use the same `--fs-detail-page-*` set because
   their typography is meant to match).
2. If not, add a new `--fs-<component>-<part>` variable next to the group it
   belongs with, and reference it with `var(...)`.
3. Never hardcode a `font-size: Npx` that changes at a breakpoint. A flat,
   non-responsive `font-size: 13px` for something that never changes size is
   fine — that's what most small, fixed-size text in the template does. What
   isn't fine is the same class ending up with a raw pixel value in one
   place and `var(--fs-...)` in another, or a raw value redefined inside
   `@media`. Both were audit findings that got cleaned up in this codebase;
   keep new code consistent with the pattern.

## 5. Color system & dark mode

Colors are also CSS variables, defined in `layout.css`:

```css
:root {
    --primary: #4b5563;
    --primary-dark: #374151;
    --secondary: #6b7280;
    --border: #d1d5db;
    --background: #f5f6f7;
    --section-bg: #eceff1;
    --surface: #ffffff;
    --text: #1f2937;
    --muted: #6b7280;
    /* ...and a few more */
}
```

`dark-mode.css` redefines the same variable names under
`:root[data-bs-theme="dark"]`, so **any element styled with `var(--...)`
goes dark automatically** — no extra work needed. The attribute is toggled
on `<html>` by `assets/js/theme-toggle.js` (Bootstrap 5.3's own dark-mode
attribute, so Bootstrap's built-in components pick it up too) and persisted
to `localStorage`.

For colors that were hardcoded instead of using a variable (a flat white
card background, a specific hex on a badge, etc.), `dark-mode.css` adds a
targeted override, organized by source file (`/* ---------- resume.css
---------- */` and so on). **If you hardcode a color in a new component,
add its dark-mode override in the matching section of `dark-mode.css`** —
otherwise it'll stay light-mode-colored when the theme switches.

Prefer using an existing `var(--...)` over a new hardcoded color whenever
the color is meant to match the template's palette; it saves you the
dark-mode override entirely.

The palette is deliberately neutral (grays + one muted blue-gray primary) —
resist the urge to add accent colors for decoration. `--danger` (the admin
panel's "Log out", the 403 page's title) is the one exception, reserved for
destructive actions and blocked/forbidden states specifically, with its
own dark-mode variant for contrast. It's a precedent for *semantic* color
additions, not an opening to start color-coding things for visual variety.

## 6. Bilingual RTL/LTR system

The template was originally written RTL-only (Farsi). Physical CSS values
(`left`, `right`, `margin-left`, forced `direction`) were converted to
**logical properties** (`inset-inline-start`, `margin-inline-end`,
`padding-inline-start`, `text-align: start`, etc.) so the exact same CSS
renders correctly in both directions — the only thing that changes between
the FA and EN pages is `dir="rtl|ltr"` and `lang="fa|en"` on `<html>`.

**When you write new CSS, always reach for the logical property, not the
physical one:**

| Instead of | Use |
|---|---|
| `left` / `right` | `inset-inline-start` / `inset-inline-end` |
| `margin-left` / `margin-right` | `margin-inline-start` / `margin-inline-end` |
| `padding-left` / `padding-right` | `padding-inline-start` / `padding-inline-end` |
| `border-left` / `border-right` | `border-inline-start` / `border-inline-end` |
| `text-align: left/right` | `text-align: start/end` |
| `border-top-left-radius`, etc. | `border-start-start-radius`, etc. |

A few values genuinely have no logical equivalent — `transform: translateX()`
and gradient angles are the two cases in this codebase. Those use the
`--flip` variable (`1` in RTL, `-1` in LTR, defined in `layout.css`) instead,
e.g. `transform: translateX(calc(var(--flip) * 4px))`. Don't invent a
direction-specific override for these; multiply by `--flip`.

**Icons:** four Bootstrap Icons classes (`bi-arrow-left`, `bi-arrow-up-left`,
`bi-box-arrow-up-left`, `bi-chevron-left`/`bi-chevron-right`) are used only
for directional meaning (forward arrows, external-link arrows, prev/next
chevrons) and are mirrored in LTR via `[dir="ltr"] .bi-arrow-left { transform:
scaleX(-1); }` in `layout.css`. The class in the HTML never changes between
FA and EN — only its rendered direction does. If you add a new directional
icon, follow the same pattern rather than swapping icon classes per
language.

**Fonts and Bootstrap build** switch automatically:
- `assets/fonts/style.css` picks IRANYekanX or Inter based on `<html lang>` —
  no change needed when you create a new page, as long as `lang` is set
  correctly.
- Bootstrap itself does **not** auto-switch: FA pages link
  `assets/vendor/bootstrap/css/bootstrap.rtl.css`, EN pages link
  `assets/vendor/bootstrap/css/bootstrap.css`. Get this right when copying
  `layout.html` (see §10) — it's the one line that has to change by hand.

## 7. Reusable components

Defined in `components.css`, usable on any page without writing new CSS:

| Component | Root class | Used on |
|---|---|---|
| Article card | `.article-card` (+ `.card-custom`) | Homepage preview grid, extended further in `articles-updated-ajax-v2.css` for the archive page |
| Project/portfolio card | `.project-card` | Homepage portfolio carousel |
| Course card | `.course-card` | Homepage courses grid |
| GitHub project card | `.github-project-card` | Homepage GitHub projects grid |
| Sidebar widget | `.sidebar-widget` | Article & project-detail sidebars (author, related items, tags) |
| Form fields | `.contact-label` / `.contact-input` / `.contact-textarea` / `.contact-submit` | Homepage contact form, article comment/reply form |
| Breadcrumb | `.post-breadcrumb` / `.project-breadcrumb` / `.courses-archive-breadcrumb` | Article, project-detail, and courses-archive pages (one shared design, one class name per page for clarity) |

Each card is a plain block of markup using these classes — see any existing
usage (e.g. the article-card markup in `Resume.html`) as the template. A
minimal example:

```html
<article class="article-card card-custom">
    <div class="article-cover"><img src="..." alt="..."></div>
    <div class="article-body">
        <h3 class="article-title">...</h3>
        <p class="article-reading-time">...</p>
    </div>
</article>
```

Also shared, but defined in `layout.css` rather than `components.css`: the
base card surface `.card-custom` (background, radius, shadow — the visual
foundation under widgets, the profile card, and content cards), and the
`site-header` / offcanvas menu / `site-footer`.

## 8. JavaScript behaviors

All under `assets/js/`. Each is self-contained and no-ops if its target
element isn't on the page, so it's safe to include the `<script>` tag on
every page.

| File | Does |
|---|---|
| `theme-init.js` | Reads the saved theme from `localStorage` and sets `data-bs-theme` on `<html>` **before** any stylesheet loads, so there's no flash of the wrong theme. Must stay as a blocking `<script src>` at the very top of `<head>`. Default theme (for first-time visitors) is set by the `DEFAULT_THEME` variable at the top of the file. |
| `theme-toggle.js` | Wires up `#themeToggleBtn`: flips `data-bs-theme`, persists it, syncs across open tabs via the `storage` event, and dispatches a `themechange` custom event that other scripts (like the particle background) listen for. |
| `particles-bg.js` | Looks for any `#particles-bg` element and renders a tsParticles background inside it; recolors on `themechange`. To use it on a new section, add an `id="particles-bg"` element inside a `position: relative` container and load `particles-bg.css` + this script — nothing else to configure. |
| `certificate-modal.js` | Powers the certificate lightbox: any button with `data-cert-image` / `data-cert-title` that opens the `#certificateModal` Bootstrap modal gets its image/caption filled in automatically. Add a new certificate by adding a new trigger button with those two attributes — no JS changes needed. |
| `articles.js` | Homepage-independent archive logic for `articles-ajax.html`: filtering, category/tag matching, sorting, and pagination, driven by a sample dataset **hardcoded at the top of the file**. In a real backend, this entire file is replaced by server-side rendering — see §14. |
| `articles-search.js` | The live search dropdown in the archive page's hero. Debounced `fetch` against `datas.json` (used here as a stand-in search endpoint), with keyboard navigation (arrow keys, <kbd>Enter</kbd>, <kbd>Esc</kbd>) and an abortable in-flight request. |
| `course-player.js` | Initializes the video.js player and wires lesson rows to it — see "Video player" below. |
| `auth-password-toggle.js` | Wires up every `.auth-password-toggle` (the eye icon on a password field): flips the input's `type`, swaps the icon, and reads its two aria-label strings from that button's `data-show-label` / `data-hide-label` attributes — no hardcoded language inside the script. |
| `auth-forgot-password.js` | Drives `ForgotPassword.html`'s 3-step wizard: shows/hides `.auth-step-panel`s, masks the phone number for step 2's confirmation text, runs the resend-code countdown, and checks new-password/confirm-password match before revealing the "done" panel. See §9a. |

### Video player

Lesson videos use [video.js](https://videojs.com), vendored locally under
`assets/vendor/video-js/` (not a CDN link, same reasoning as Bootstrap) and
re-skinned in `assets/css/video-player.css` to use the template's own color
variables instead of video.js's default dark-gray chrome. Currently only
`CourseDetails.html` / `.en.html` use it, but it's built to be reusable
anywhere else a video is needed:

1. Link `assets/vendor/video-js/video-js.min.css`, `assets/css/video-player.css`,
   `assets/vendor/video-js/video.min.js`, and `assets/js/course-player.js`
   (in that order, the two CSS files early in `<head>`, the two scripts at
   the end of `<body>`, same pattern as the other pages).
2. Add a `<video id="coursePlayer" class="video-js" controls preload="metadata">`
   with a `<source>` inside, wrapped in a `.course-player-wrap` div.
3. For a clickable playlist (like the curriculum accordion here), give each
   trigger element a `data-video-src` / `data-video-title` pair and a
   `.course-lesson-play` button inside it — `course-player.js` finds every
   `.course-lesson-item[data-video-src]` automatically and swaps the
   player's source + an optional `#coursePlayerCaptionText` caption on
   click. No per-page JS needed.

The current lesson videos all point to the same public domain sample file
(Google's test-media bucket) as a stand-in — swap `data-video-src` (and the
`<source>` default) for real lesson URLs when they exist. The per-lesson
"download" buttons point to the same placeholder; the "download attachment"
button only appears on lessons that have one (`assets/downloads/` holds a
placeholder ZIP — replace with real per-lesson files and update the `href`).

## 9. Admin panel

The admin section (`AdminLayout.html` / `.en.html`) is a separate shell from
the public site — sidebar + header + footer instead of the public site's
top nav + hero, matching common admin/dashboard conventions while reusing
the exact same tokens (colors, fonts, `.card-custom`) as everywhere else.
Not a real page — like `layout.html`, it's a starting point, and doubles as
its own sample "Dashboard" page.

`assets/css/admin-layout.css` holds everything sidebar/header/footer
specific; `layout.css` still supplies `:root` variables, the reset, and
`.card-custom`. `components.css` isn't loaded — none of its cards apply in
an admin shell.

**Sidebar**: a fixed-width column on desktop (≥992px) with a brand mark, a
collapse-to-icons toggle (persisted to `localStorage` by
`assets/js/admin-sidebar.js`, the same pattern as the theme toggle), and a
nav list. Below 992px it becomes a Bootstrap offcanvas — literally the same
`.site-offcanvas` surface styling the public mobile nav uses, triggered by
a hamburger button in the header. **Keep the desktop `<aside>` and the
offcanvas `<div>`'s nav links in sync** — they're two copies of the same
menu, not two different menus.

**Adding a real admin page**: copy `AdminLayout.html` / `.en.html`, then:
1. Update `<title>` and the header eyebrow/`<h1>`.
2. Move `class="is-active"` to the new page's nav link — in **both** the
   desktop sidebar and the offcanvas copy of it.
3. Replace the `admin-content` section with the real content.

Sidebar, header chrome, and footer stay as they are.

**Mobile is the priority here, not an afterthought**: test any admin page
at a narrow viewport before considering it done. The collapse toggle is
desktop-only, but the offcanvas menu, header spacing, and content reflow
all need to hold up on a phone.

## 9a. Auth pages

`Login.html`, `Signup.html`, and `ForgotPassword.html` (+ `.en.html`) are
a separate family from both the public site and the admin panel: no
header, footer, or offcanvas at all — just the floating theme-toggle
button and one centered `.auth-card` (`.card-custom` underneath) on the
page's normal `var(--background)`, with the same `#particles-bg` layer
Resume.html's hero uses. All three share `assets/css/auth.css` on
purpose, so a change to the card, the brand row, or a form field looks
identical on all three instead of drifting apart.

Recovery is phone-based (`autocomplete="tel"`, `dir="ltr"` on the input,
same convention as `.github-stars`) because Signup never collects an
email — Login's identifier matches Signup's for the same reason.

`ForgotPassword.html` is a 3-step wizard in one card (phone → code → new
password), plus a final "done" panel, all toggled by
`assets/js/auth-forgot-password.js` via `.d-none` on each
`.auth-step-panel`; nothing here talks to a real backend yet — the OTP
and phone lookups are not actually checked against anything, only the
new-password/confirm-password match is (client-side, no backend needed
for that one). `assets/js/auth-password-toggle.js` (the show/hide-password
eye icon) is shared by all three pages and reads its two aria-label
strings from `data-show-label` / `data-hide-label` on the button, so the
one script works unchanged on both languages — don't hardcode either
string inside the script itself.

One RTL/LTR exception worth knowing about: the back-link icon on step 2
means "back", the opposite of what the sitewide `bi-arrow-left` mirror
list means ("forward") — so it uses `bi-arrow-right` in FA and
`bi-arrow-left` in EN, with a small override in `auth.css` that cancels
the automatic mirror for that one icon so it doesn't get flipped back
to pointing right. If you add another back/previous icon anywhere,
follow this same pattern rather than reusing the sitewide mirror as-is.

## 10. How to add a new page

1. **Copy the starting-point template**, not an existing content page:
   - FA page: copy `layout.html`.
   - EN page: copy `layout.en.html`.
2. Rename the file, update `<title>` and the meta description.
3. Create a page-specific CSS file under `assets/css/` (e.g.
   `my-new-page.css`) and link it after `layout.css` and `components.css`,
   exactly where `layout.html` already has a placeholder `<link>` for it.
4. Replace the `PAGE CONTENT` section (inside `<main>`) with the real
   content. **Leave the `<head>` block, theme toggle button, site header,
   mobile offcanvas menu, and footer as they are** — those five pieces are
   identical on every page and already wired up correctly (dark mode,
   RTL/LTR, nav links back to the homepage).
5. If the new page needs a hero-style particle background, add
   `id="particles-bg"` inside a `position: relative` container and link
   `particles-bg.css` (see §8).
6. Building an EN version too? Keep the HTML structure and classes
   identical to the FA version — only the visible text, `dir`/`lang`, and
   the Bootstrap CSS link should differ (see §6). `layout.en.html` documents
   the exact three differences from `layout.html` at the top of the file.

`CourseDetails.html` / `CourseDetails.en.html` (+ `course-details.css`) are a
worked example of this exact recipe — a "detail page" for a single course,
built the same way `ProjectDetails.html` was for a single project: shared
`--fs-detail-page-*` typography (§4), the existing `.author-widget` /
`.widget-list` / breadcrumb components reused as-is (§7), and only the
genuinely course-specific pieces (highlights checklist, curriculum
accordion) written as new CSS. Worth reading alongside this section if
you're about to build something similar.

## 11. How to add a new section to an existing page

Most homepage sections (Skills, Timeline, Courses, GitHub Projects,
Portfolio, Certificates, Articles, Contact) follow the same header pattern —
reuse it rather than inventing a new one:

```html
<div class="section-header">
    <div class="section-eyebrow">
        <span class="section-number">07</span>
        <span class="section-label">Category label</span>
    </div>
    <h2 class="section-title">Section title</h2>
    <p class="section-description">One-line description.</p>
</div>
```

Need a "view all" link inline with the title (used by Courses, GitHub
Projects, Portfolio, Articles)? Use the extended variant instead — it
handles RTL/LTR placement automatically:

```html
<div class="section-header section-header--with-action">
    <div class="section-header-main">
        <!-- same eyebrow / title / description as above -->
    </div>
    <a class="section-view-all" href="...">
        View all
        <i class="bi bi-arrow-left"></i>
    </a>
</div>
```

For the content below the header, reach for an existing component (§7)
before writing new CSS.

## 12. How to add a new reusable component

If you're building something that belongs in `components.css` rather than a
page-specific file (see the rule of thumb in §3):

1. Add the markup and CSS following an existing component as a template —
   `.course-card` is a good one to copy since it's fully self-contained
   (cover image, overlay, badge, body, title, link).
2. Use logical properties throughout (§6), and `var(--...)` for any color
   that should match the template palette (§5) — this gets you dark-mode
   support for free.
3. If any text needs to scale responsively, add a `--fs-*` variable in
   `layout.css` rather than hardcoding breakpoint-specific sizes (§4).
4. If you do hardcode a color instead of using a variable, add the
   corresponding override in `dark-mode.css`.
5. Test the component on both a FA/RTL page and an EN/LTR page before
   considering it done.

## 13. Known dead code

`resume.css` has a `.decor-left` / `.decor-right` rule block (decorative
side images near the hero) that isn't referenced by any current HTML. It's
left in place intentionally rather than removed, in case a future design
pass reintroduces the decoration — if you do use it, double-check the
logical-property conversion against real markup at that point, since it was
never exercised in production.

## 14. Possible future backend migration

The footer of every page notes this template is meant to be wired up to an
ASP.NET Core backend. If/when that happens, the static structure maps
fairly directly onto Razor:

| Static HTML today | Razor equivalent |
|---|---|
| `<head>` + header + offcanvas + footer (identical on every page) | `_Layout.cshtml` |
| Site header block | Partial view: `_Header.cshtml` |
| Mobile offcanvas menu | Partial view: `_OffcanvasNav.cshtml` |
| Site footer | Partial view: `_Footer.cshtml` |
| `PAGE CONTENT` section in `layout.html` | `@RenderBody()` |
| `layout.css` / `components.css` `<link>`s | Stay in `_Layout.cshtml` (shared) |
| Page's own CSS `<link>` | Moves into `@section Styles` on each view |
| FA text inside header/offcanvas/footer | `@Localizer["Key"]` |
| The three FA/EN differences (§6, §10) | One shared layout with `@culture`, e.g.:<br>`<html lang="@culture" dir="@(culture=="fa" ? "rtl" : "ltr")">`, and a simple `@if` around the Bootstrap `<link>` |

Until that middleware exists, the FA and EN pages stay two static, parallel
file sets rather than one templated view — that's intentional, not a
shortcut to fix.

`articles.js`'s hardcoded sample array and `datas.json` both stand in for
what would become real API/database calls; the client-side filter/sort/
pagination logic in `articles.js` would move server-side at that point.

## 15. Checklist before committing

- [ ] New font sizes use an existing or new `--fs-*` variable if they change
      across breakpoints — no raw `font-size: Npx` inside a media query.
- [ ] New colors use an existing `var(--...)` where possible; any new
      hardcoded color has a `dark-mode.css` override.
- [ ] New spacing/positioning uses logical properties, not `left`/`right`/
      `margin-left`/etc.
- [ ] Checked the page in both light and dark mode.
- [ ] Checked the page in both an RTL (FA) and LTR (EN) page if the change
      touches shared CSS (`layout.css`, `components.css`, or a component
      used on both language variants).
- [ ] Comments explain *why*, not *what* — keep them short and in English.
