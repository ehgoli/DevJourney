/*
 * Courses archive — client-side level filter.
 * Only a handful of courses exist, so this filters what's already in the
 * DOM (data-course-level on each grid item) rather than fetching anything.
 */
document.addEventListener('DOMContentLoaded', function () {
    var pills = document.querySelectorAll('.courses-filter-pill');
    var items = document.querySelectorAll('.courses-archive-grid-item');
    var emptyState = document.querySelector('.courses-archive-empty');

    if (!pills.length || !items.length) {
        return;
    }

    function applyFilter(level) {
        var visibleCount = 0;

        items.forEach(function (item) {
            var matches = level === 'all' || item.getAttribute('data-course-level') === level;
            item.hidden = !matches;
            if (matches) {
                visibleCount += 1;
            }
        });

        if (emptyState) {
            emptyState.classList.toggle('is-visible', visibleCount === 0);
        }
    }

    pills.forEach(function (pill) {
        pill.addEventListener('click', function () {
            pills.forEach(function (p) {
                p.classList.remove('is-active');
                p.setAttribute('aria-pressed', 'false');
            });
            pill.classList.add('is-active');
            pill.setAttribute('aria-pressed', 'true');
            applyFilter(pill.getAttribute('data-filter'));
        });
    });
});
