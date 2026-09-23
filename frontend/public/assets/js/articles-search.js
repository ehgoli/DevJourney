(() => {
    "use strict";

    const form = document.getElementById('articlesSearchForm');
    const input = document.getElementById('articlesSearchInput');
    const results = document.getElementById('articlesSearchResults');
    const status = document.getElementById('articlesSearchStatus');
    const clearButton = document.getElementById('articlesSearchClear');
    const loader = document.getElementById('articlesSearchLoader');
    const submitButton = document.getElementById('articlesSearchSubmit');

    if (!form || !input || !results) return;

    const MIN_QUERY_LENGTH = 2;
    const DEBOUNCE_MS = 280;
    let debounceTimer = null;
    let activeController = null;
    let activeItems = [];
    let activeIndex = -1;

    const escapeHtml = (value) => String(value ?? '')
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#039;');

    const normalizeResponse = (payload) => {
        if (Array.isArray(payload)) return payload;
        if (!payload || typeof payload !== 'object') return [];
        for (const key of ['data', 'items', 'results', 'articles', 'content']) {
            if (Array.isArray(payload[key])) return payload[key];
        }
        return [];
    };

    const getTitle = (item) => item?.title ?? item?.name ?? item?.articleTitle ?? 'بدون عنوان';
    const getSummary = (item) => item?.summary ?? item?.excerpt ?? item?.description ?? item?.content ?? '';
    const getCategory = (item) => item?.categoryName ?? item?.category ?? item?.topic ?? '';
    const getMeta = (item) => item?.readingTime ?? item?.readingTimeMinutes ? `${item.readingTime ?? item.readingTimeMinutes + ' دقیقه مطالعه'}` : '';
    const getUrl = (item) => item?.url ?? item?.link ?? (item?.slug ? `article.html?slug=${encodeURIComponent(item.slug)}` : 'article.html');

    const setLoading = (isLoading) => {
        form.classList.toggle('is-loading', isLoading);
        loader.hidden = !isLoading;
        submitButton.disabled = isLoading;
    };

    const setStatus = (message = '') => {
        status.textContent = message;
        status.hidden = !message;
    };

    const closeResults = () => {
        results.hidden = true;
        input.setAttribute('aria-expanded', 'false');
        activeItems = [];
        activeIndex = -1;
    };

    const clearResults = () => {
        results.innerHTML = '';
        closeResults();
        setStatus('');
    };

    const highlight = (text, query) => {
        const safe = escapeHtml(text);
        const tokens = query.trim().split(/\s+/).filter(Boolean).slice(0, 6);
        if (!tokens.length) return safe;
        const pattern = tokens.map((token) => token.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')).join('|');
        return safe.replace(new RegExp(`(${pattern})`, 'gi'), '<mark>$1</mark>');
    };

    const renderResults = (items, query) => {
        activeItems = items.slice(0, 8);
        activeIndex = -1;

        if (!activeItems.length) {
            results.innerHTML = `
                <div class="articles-search-empty">
                    <span class="articles-search-result-icon"><i class="bi bi-search"></i></span>
                    <div><strong>نتیجه‌ای پیدا نشد</strong><small>عبارت «${escapeHtml(query)}» را با کلیدواژه دیگری امتحان کن.</small></div>
                </div>`;
            results.hidden = false;
            input.setAttribute('aria-expanded', 'true');
            setStatus('نتیجه‌ای برای این جستجو پیدا نشد.');
            return;
        }

        results.innerHTML = activeItems.map((item, index) => {
            const title = getTitle(item);
            const summary = getSummary(item);
            const category = getCategory(item);
            const meta = getMeta(item);
            const url = getUrl(item);
            return `
                <a class="articles-search-result" id="articles-search-result-${index}" href="${escapeHtml(url)}" role="option" aria-selected="false" data-index="${index}">
                    <span class="articles-search-result-icon"><i class="bi bi-journal-text"></i></span>
                    <span class="articles-search-result-copy">
                        <strong>${highlight(title, query)}</strong>
                        ${summary ? `<small>${escapeHtml(summary).slice(0, 150)}${summary.length > 150 ? '…' : ''}</small>` : ''}
                        <span class="articles-search-result-meta">
                            ${category ? `<span><i class="bi bi-bookmark"></i>${escapeHtml(category)}</span>` : ''}
                            ${meta ? `<span><i class="bi bi-clock"></i>${escapeHtml(meta)}</span>` : ''}
                        </span>
                    </span>
                    <i class="bi bi-arrow-up-left articles-search-result-arrow" aria-hidden="true"></i>
                </a>`;
        }).join('');

        results.hidden = false;
        input.setAttribute('aria-expanded', 'true');
        setStatus(`${activeItems.length} نتیجه در جستجوی زنده پیدا شد.`);
    };

    const searchArticles = async (rawQuery) => {
        const query = rawQuery.trim();
        clearResults();
        if (query.length < MIN_QUERY_LENGTH) { setLoading(false); return; }

        if (activeController) activeController.abort();
        activeController = new AbortController();
        setLoading(true);
        setStatus('در حال جستجو…');

        try {
            const response = await fetch(`/datas.json?name=${encodeURIComponent(query)}`, {
                method: 'GET',
                headers: { Accept: 'application/json' },
                signal: activeController.signal
            });
            if (!response.ok) throw new Error(`HTTP ${response.status}`);
            const payload = await response.json();
            renderResults(normalizeResponse(payload), query);
        } catch (error) {
            if (error.name === 'AbortError') return;
            results.innerHTML = `
                <div class="articles-search-error">
                    <span class="articles-search-result-icon"><i class="bi bi-wifi-off"></i></span>
                    <div><strong>جستجو موقتاً در دسترس نیست</strong><small>ارتباط با سرویس جستجو برقرار نشد. دوباره امتحان کن.</small></div>
                    <button type="button" class="articles-search-retry" id="articlesSearchRetry">تلاش مجدد</button>
                </div>`;
            results.hidden = false;
            input.setAttribute('aria-expanded', 'true');
            setStatus('خطا در دریافت نتایج.');
            document.getElementById('articlesSearchRetry')?.addEventListener('click', () => searchArticles(input.value));
        } finally {
            setLoading(false);
        }
    };

    const scheduleSearch = () => {
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(() => searchArticles(input.value), DEBOUNCE_MS);
    };

    const updateClearButton = () => { clearButton.hidden = input.value.length === 0; };

    input.addEventListener('input', () => { updateClearButton(); scheduleSearch(); });
    input.addEventListener('focus', () => {
        if (results.innerHTML.trim() && input.value.trim().length >= MIN_QUERY_LENGTH) {
            results.hidden = false;
            input.setAttribute('aria-expanded', 'true');
        }
    });

    input.addEventListener('keydown', (event) => {
        if (event.key === 'Escape') { closeResults(); return; }
        if (!activeItems.length || results.hidden) return;
        if (event.key === 'ArrowDown') { event.preventDefault(); activeIndex = (activeIndex + 1) % activeItems.length; }
        else if (event.key === 'ArrowUp') { event.preventDefault(); activeIndex = (activeIndex - 1 + activeItems.length) % activeItems.length; }
        else if (event.key === 'Enter' && activeIndex >= 0) {
            event.preventDefault();
            document.getElementById(`articles-search-result-${activeIndex}`)?.click();
            return;
        } else return;

        document.querySelectorAll('.articles-search-result').forEach((el, index) => {
            const active = index === activeIndex;
            el.classList.toggle('is-active', active);
            el.setAttribute('aria-selected', String(active));
        });
    });

    clearButton.addEventListener('click', () => {
        if (activeController) activeController.abort();
        input.value = '';
        updateClearButton();
        clearResults();
        input.focus();
    });

    form.addEventListener('submit', (event) => {
        event.preventDefault();
        if (input.value.trim().length >= MIN_QUERY_LENGTH) searchArticles(input.value);
    });

    document.addEventListener('click', (event) => {
        if (!form.contains(event.target)) closeResults();
    });

    updateClearButton();
})();
