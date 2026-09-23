(() => {
    "use strict";

    document.addEventListener("DOMContentLoaded", () => {

        /*
         * =====================================================
         * ARTICLES ARCHIVE
         * Search + Category Filter + Sort + Pagination
         * =====================================================
         */

        const articles = [
            {
                id: 1,
                title: "چرا Clean Architecture را انتخاب کردم؟",
                excerpt:
                    "یادداشتی درباره تجربه انتخاب Clean Architecture در پروژه‌های واقعی، مزایا، چالش‌ها و نکاتی که در این مسیر یاد گرفتم.",
                category: "معماری نرم‌افزار",
                tags: [
                    "Clean Architecture",
                    "Domain",
                    "EF Core"
                ],
                date: "2026-08-10",
                dateFa: "۱۰ مرداد ۱۴۰۵",
                reading: 5,
                image: "assets/images/default-course-cover.jpg",
                href: "article.html"
            },

            {
                id: 2,
                title: "CQRS در پروژه‌های واقعی",
                excerpt:
                    "نگاهی کاربردی به جداسازی Command و Query و جایی که CQRS واقعاً در پروژه ارزش ایجاد می‌کند.",
                category: "Backend",
                tags: [
                    "CQRS",
                    "MediatR",
                    "Backend"
                ],
                date: "2026-08-02",
                dateFa: "۲ مرداد ۱۴۰۵",
                reading: 7,
                image: "assets/images/default-course-cover.jpg",
                href: "#"
            },

            {
                id: 3,
                title: "از MVC تا Minimal API",
                excerpt:
                    "مقایسه‌ای از تجربه توسعه API در ASP.NET Core و اینکه چه زمانی Minimal API انتخاب بهتری است.",
                category: "توسعه وب",
                tags: [
                    "ASP.NET Core",
                    "Minimal API",
                    "MVC"
                ],
                date: "2026-07-29",
                dateFa: "۲۹ تیر ۱۴۰۵",
                reading: 6,
                image: "assets/images/default-course-cover.jpg",
                href: "#"
            },

            {
                id: 4,
                title: "Dependency Injection در ASP.NET Core",
                excerpt:
                    "Dependency Injection یکی از مفاهیم مهم در توسعه برنامه‌های ASP.NET Core است و در این نوشته نگاهی کاربردی به آن داریم.",
                category: "توسعه وب",
                tags: [
                    "ASP.NET Core",
                    "Dependency Injection",
                    "C#"
                ],
                date: "2026-07-20",
                dateFa: "۲۰ تیر ۱۴۰۵",
                reading: 5,
                image: "assets/images/default-course-cover.jpg",
                href: "#"
            },

            {
                id: 5,
                title: "چطور یک Backend قابل نگهداری طراحی کنیم؟",
                excerpt:
                    "نگاهی به اصولی که می‌توانند باعث شوند کد Backend در پروژه‌های واقعی خواناتر، تست‌پذیرتر و قابل نگهداری‌تر باشد.",
                category: "Backend",
                tags: [
                    "Backend",
                    "Architecture",
                    "Clean Code"
                ],
                date: "2026-07-12",
                dateFa: "۱۲ تیر ۱۴۰۵",
                reading: 6,
                image: "assets/images/default-course-cover.jpg",
                href: "#"
            },

            {
                id: 6,
                title: "CQRS چیست و چه زمانی باید از آن استفاده کنیم؟",
                excerpt:
                    "در این مقاله با مفهوم CQRS، مزایا، معایب و شرایطی که استفاده از آن می‌تواند منطقی باشد آشنا می‌شویم.",
                category: "معماری نرم‌افزار",
                tags: [
                    "CQRS",
                    "Architecture",
                    "MediatR"
                ],
                date: "2026-07-05",
                dateFa: "۵ تیر ۱۴۰۵",
                reading: 8,
                image: "assets/images/default-course-cover.jpg",
                href: "#"
            }
        ];


        /*
         * =====================================================
         * STATE
         * =====================================================
         */

        const state = {
            query: "",
            category: "all",
            sort: "newest",
            page: 1,
            perPage: 3
        };


        /*
         * =====================================================
         * ELEMENTS
         * =====================================================
         */

        const searchInput =
            document.getElementById("articleSearch");

        const clearSearch =
            document.getElementById("clearSearch");

        const sortSelect =
            document.getElementById("sortArticles");

        const grid =
            document.getElementById("articlesGrid");

        const emptyState =
            document.getElementById("emptyState");

        const paginationWrap =
            document.getElementById("paginationWrap");

        const summary =
            document.getElementById("resultSummary");

        const resetButton =
            document.getElementById("resetFilters");

        const emptyReset =
            document.getElementById("emptyReset");

        const totalArticles =
            document.getElementById("totalArticles");

        const area =
            document.getElementById("articlesArea");


        /*
         * =====================================================
         * SAFETY CHECK
         * =====================================================
         */

        if (
            !searchInput ||
            !clearSearch ||
            !sortSelect ||
            !grid ||
            !emptyState ||
            !paginationWrap ||
            !summary ||
            !resetButton ||
            !emptyReset ||
            !totalArticles ||
            !area
        ) {
            console.error(
                "Articles Archive: One or more required HTML elements were not found."
            );

            return;
        }


        /*
         * =====================================================
         * HELPERS
         * =====================================================
         */

        function faDigits(value) {

            return String(value).replace(
                /\d/g,
                digit => "۰۱۲۳۴۵۶۷۸۹"[digit]
            );
        }


        function normalize(value) {

            return String(value || "")
                .trim()
                .toLocaleLowerCase("fa-IR")
                .replace(/[يى]/g, "ی")
                .replace(/ك/g, "ک");
        }


        function escapeHTML(value) {

            return String(value || "")
                .replace(/&/g, "&amp;")
                .replace(/</g, "&lt;")
                .replace(/>/g, "&gt;")
                .replace(/"/g, "&quot;")
                .replace(/'/g, "&#039;");
        }


        /*
         * =====================================================
         * FILTER
         * =====================================================
         */

        function matches(article) {

            const query =
                normalize(state.query);

            const searchableText =
                normalize(
                    [
                        article.title,
                        article.excerpt,
                        article.category,
                        ...(article.tags || [])
                    ].join(" ")
                );


            const queryMatch =
                !query ||
                searchableText.includes(query);


            const categoryMatch =
                state.category === "all" ||
                article.category === state.category;


            return (
                queryMatch &&
                categoryMatch
            );
        }


        /*
         * =====================================================
         * SORT
         * =====================================================
         */

        function getFilteredArticles() {

            const result =
                articles
                    .filter(matches)
                    .slice();


            if (state.sort === "oldest") {

                result.sort(
                    (a, b) =>
                        a.date.localeCompare(b.date)
                );

            } else if (state.sort === "reading") {

                result.sort(
                    (a, b) =>
                        a.reading - b.reading ||
                        b.date.localeCompare(a.date)
                );

            } else {

                result.sort(
                    (a, b) =>
                        b.date.localeCompare(a.date)
                );
            }


            return result;
        }


        /*
         * =====================================================
         * ARTICLE CARD
         * =====================================================
         */

        function createArticleCard(article) {

            return `
                <div class="col-xl-4 col-lg-4 col-md-6">

                    <article class="article-card">

                        <a
                            class="archive-cover-link"
                            href="${escapeHTML(article.href)}"
                            aria-label="مطالعه ${escapeHTML(article.title)}"
                        >

                            <div class="article-cover">

                                <img
                                    src="${escapeHTML(article.image)}"
                                    alt="${escapeHTML(article.title)}"
                                    loading="lazy"
                                >

                            </div>

                        </a>


                        <div class="article-body">

                            <div class="archive-card-meta">

                                <span class="archive-card-category">
                                    ${escapeHTML(article.category)}
                                </span>

                                <span class="archive-card-date">
                                    ${escapeHTML(article.dateFa)}
                                </span>

                            </div>


                            <h2 class="article-title">

                                <a
                                    href="${escapeHTML(article.href)}"
                                    class="text-reset"
                                >
                                    ${escapeHTML(article.title)}
                                </a>

                            </h2>


                            <p class="articles-description">
                                ${escapeHTML(article.excerpt)}
                            </p>


                            <div class="article-reading-time">

                                <i class="bi bi-clock ms-1"></i>

                                ${faDigits(article.reading)}
                                دقیقه مطالعه

                            </div>


                            <a
                                class="article-link"
                                href="${escapeHTML(article.href)}"
                            >
                                مطالعه مقاله

                                <i class="bi bi-arrow-left"></i>
                            </a>

                        </div>

                    </article>

                </div>
            `;
        }


        /*
         * =====================================================
         * PAGINATION BUTTON
         * =====================================================
         */

        function createPageButton(
            page,
            label,
            disabled = false,
            active = false
        ) {

            return `
                <button
                    class="archive-page-link ${
                        active
                            ? "is-active"
                            : ""
                    }"
                    type="button"
                    data-page="${page}"
                    ${
                        disabled
                            ? "disabled"
                            : ""
                    }
                    ${
                        active
                            ? 'aria-current="page"'
                            : ""
                    }
                >
                    ${label}
                </button>
            `;
        }


        /*
         * =====================================================
         * PAGINATION
         * =====================================================
         */

        function renderPagination(
            totalPages
        ) {

            if (totalPages <= 1) {

                paginationWrap.innerHTML = "";

                return;
            }


            let html = "";


            // Previous
            html += createPageButton(
                state.page - 1,
                '<i class="bi bi-chevron-right"></i>',
                state.page === 1
            );


            // Pages
            if (totalPages <= 7) {

                for (
                    let page = 1;
                    page <= totalPages;
                    page++
                ) {

                    html += createPageButton(
                        page,
                        faDigits(page),
                        false,
                        page === state.page
                    );
                }

            } else {

                html += createPageButton(
                    1,
                    faDigits(1),
                    false,
                    state.page === 1
                );


                if (state.page > 4) {

                    html += `
                        <span class="archive-page-link is-dots">
                            …
                        </span>
                    `;
                }


                const start =
                    Math.max(
                        2,
                        state.page - 1
                    );

                const end =
                    Math.min(
                        totalPages - 1,
                        state.page + 1
                    );


                for (
                    let page = start;
                    page <= end;
                    page++
                ) {

                    html += createPageButton(
                        page,
                        faDigits(page),
                        false,
                        page === state.page
                    );
                }


                if (
                    state.page <
                    totalPages - 3
                ) {

                    html += `
                        <span class="archive-page-link is-dots">
                            …
                        </span>
                    `;
                }


                html += createPageButton(
                    totalPages,
                    faDigits(totalPages),
                    false,
                    state.page === totalPages
                );
            }


            // Next
            html += createPageButton(
                state.page + 1,
                '<i class="bi bi-chevron-left"></i>',
                state.page === totalPages
            );


            paginationWrap.innerHTML = `
                <div class="archive-pagination">
                    ${html}
                </div>
            `;
        }


        /*
         * =====================================================
         * RENDER
         * =====================================================
         */

        function render() {

            const result =
                getFilteredArticles();


            const totalPages =
                Math.max(
                    1,
                    Math.ceil(
                        result.length /
                        state.perPage
                    )
                );


            if (
                state.page >
                totalPages
            ) {

                state.page =
                    totalPages;
            }


            const start =
                (state.page - 1) *
                state.perPage;


            const currentItems =
                result.slice(
                    start,
                    start + state.perPage
                );


            /*
             * Cards
             */

            grid.innerHTML =
                currentItems
                    .map(createArticleCard)
                    .join("");


            const hasResults =
                currentItems.length > 0;


            /*
             * Empty state
             */

            grid.hidden =
                !hasResults;

            emptyState.hidden =
                hasResults;


            /*
             * Pagination
             */

            paginationWrap.hidden =
                !hasResults;


            /*
             * Result summary
             */

            if (hasResults) {

                summary.innerHTML = `
                    <strong>
                        ${faDigits(result.length)}
                    </strong>

                    مقاله

                    ${
                        totalPages > 1
                            ? `
                                • صفحه
                                ${faDigits(state.page)}
                                از
                                ${faDigits(totalPages)}
                              `
                            : ""
                    }
                `;

            } else {

                summary.textContent =
                    "۰ نتیجه برای جست‌وجوی فعلی";
            }


            /*
             * Search clear button
             */

            clearSearch.hidden =
                !state.query;


            /*
             * Total articles
             */

            totalArticles.textContent =
                faDigits(articles.length);


            /*
             * Pagination render
             */

            renderPagination(
                totalPages
            );


            /*
             * Loading state
             */

            area.setAttribute(
                "aria-busy",
                "false"
            );
        }


        /*
         * =====================================================
         * RESET
         * =====================================================
         */

        function reset() {

            state.query = "";
            state.category = "all";
            state.sort = "newest";
            state.page = 1;


            searchInput.value = "";

            sortSelect.value =
                "newest";


            document
                .querySelectorAll(
                    ".archive-filter"
                )
                .forEach(button => {

                    button.classList.toggle(
                        "is-active",
                        button.dataset.category ===
                        "all"
                    );
                });


            render();


            document
                .querySelector(
                    ".archive-toolbar"
                )
                ?.scrollIntoView({
                    behavior: "smooth",
                    block: "start"
                });
        }


        /*
         * =====================================================
         * SEARCH
         * =====================================================
         */

        searchInput.addEventListener(
            "input",
            event => {

                state.query =
                    event.target.value;

                state.page = 1;

                render();
            }
        );


        /*
         * =====================================================
         * CLEAR SEARCH
         * =====================================================
         */

        clearSearch.addEventListener(
            "click",
            () => {

                searchInput.value = "";

                state.query = "";

                state.page = 1;

                render();

                searchInput.focus();
            }
        );


        /*
         * =====================================================
         * SORT
         * =====================================================
         */

        sortSelect.addEventListener(
            "change",
            event => {

                state.sort =
                    event.target.value;

                state.page = 1;

                render();
            }
        );


        /*
         * =====================================================
         * CATEGORY FILTER
         * =====================================================
         */

        document
            .querySelectorAll(
                ".archive-filter"
            )
            .forEach(button => {

                button.addEventListener(
                    "click",
                    () => {

                        state.category =
                            button.dataset.category;

                        state.page = 1;


                        document
                            .querySelectorAll(
                                ".archive-filter"
                            )
                            .forEach(item => {

                                item.classList.remove(
                                    "is-active"
                                );
                            });


                        button.classList.add(
                            "is-active"
                        );


                        render();
                    }
                );
            });


        /*
         * =====================================================
         * RESET BUTTONS
         * =====================================================
         */

        resetButton.addEventListener(
            "click",
            reset
        );


        emptyReset.addEventListener(
            "click",
            reset
        );


        /*
         * =====================================================
         * PAGINATION CLICK
         * =====================================================
         */

        paginationWrap.addEventListener(
            "click",
            event => {

                const button =
                    event.target.closest(
                        "[data-page]"
                    );


                if (
                    !button ||
                    button.disabled
                ) {
                    return;
                }


                const page =
                    Number(
                        button.dataset.page
                    );


                if (
                    !Number.isFinite(page) ||
                    page < 1
                ) {
                    return;
                }


                state.page = page;

                render();


                document
                    .querySelector(
                        ".archive-toolbar"
                    )
                    ?.scrollIntoView({
                        behavior: "smooth",
                        block: "start"
                    });
            }
        );


        /*
         * =====================================================
         * INITIALIZE
         * =====================================================
         */

        totalArticles.textContent =
            faDigits(articles.length);

        render();

    });

})();