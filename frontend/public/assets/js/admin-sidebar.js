/*
 * Admin sidebar behaviors
 * -------------------------------------------------------
 * 1) Desktop collapse toggle (icon-only mode). The mobile sidebar is a
 *    Bootstrap offcanvas and needs no JS of its own for that. The
 *    collapsed/expanded state is persisted per browser, the same way
 *    theme-toggle.js persists the color theme.
 *
 * 2) Nested menus. Opening/closing a group is Bootstrap's own collapse
 *    (data-bs-toggle="collapse" in the markup; one top-level group is
 *    open at a time through data-bs-parent). On top of that, this file:
 *      a) opens the group(s) that hold the current page — put
 *         class="is-active" on the link and its parent groups open
 *         by themselves, in both the desktop and the mobile copy;
 *      b) lets a click on a group icon expand the icon-only sidebar and
 *         open that group in one go;
 *      c) scrolls a freshly opened group into view, so on a phone its
 *         items are not left hidden below the fold of the menu.
 */
document.addEventListener('DOMContentLoaded', function () {
    var shell = document.querySelector('.admin-shell');
    if (!shell) {
        return;
    }

    var STORAGE_KEY = 'adminSidebarCollapsed';
    var toggleBtn = document.getElementById('adminSidebarCollapseBtn');

    function readCollapsed() {
        try {
            return localStorage.getItem(STORAGE_KEY) === '1';
        } catch (e) {
            return false;
        }
    }

    function saveCollapsed(collapsed) {
        try {
            localStorage.setItem(STORAGE_KEY, collapsed ? '1' : '0');
        } catch (e) {
            /* localStorage unavailable (private mode, etc.) — the state just isn't remembered */
        }
    }

    function setCollapsed(collapsed) {
        shell.classList.toggle('is-collapsed', collapsed);
        if (toggleBtn) {
            toggleBtn.setAttribute('aria-expanded', String(!collapsed));
        }
    }

    /* ---------- 1) Desktop collapse toggle ---------- */

    if (toggleBtn) {
        setCollapsed(readCollapsed());

        toggleBtn.addEventListener('click', function () {
            var collapsed = !shell.classList.contains('is-collapsed');
            setCollapsed(collapsed);
            saveCollapsed(collapsed);
        });
    }

    /* ---------- 2) Nested menus ---------- */

    // a) Open the group(s) that hold the current page's link.
    document.querySelectorAll('.admin-nav .is-active').forEach(function (link) {
        var panel = link.closest('.admin-subnav');

        while (panel) {
            panel.classList.add('show');

            document.querySelectorAll('[data-bs-target="#' + panel.id + '"]').forEach(function (trigger) {
                trigger.classList.remove('collapsed');
                trigger.classList.add('has-active');
                trigger.setAttribute('aria-expanded', 'true');
            });

            panel = panel.parentElement.closest('.admin-subnav');
        }
    });

    // b) Icon-only sidebar: a click on a group icon expands the sidebar
    //    and opens that group.
    //
    //    Bootstrap registers its delegated click handlers on `document` in
    //    the *capture* phase, so a normal (bubbling) listener would run
    //    after Bootstrap has already toggled the group. This one is
    //    therefore registered on `window` in the capture phase, which runs
    //    first: it expands the sidebar, then lets Bootstrap open the group
    //    (and close the previously open one) as usual.
    window.addEventListener('click', function (event) {
        var trigger = event.target.closest && event.target.closest('.admin-sidebar [data-bs-toggle="collapse"]');

        if (!trigger || !shell.classList.contains('is-collapsed')) {
            return;
        }

        setCollapsed(false);
        saveCollapsed(false);

        // A group that holds the current page is already open (just hidden
        // by CSS while the sidebar is icon-only). Bootstrap would *toggle*
        // it closed, so for that one case Bootstrap's handler is skipped.
        var panel = document.querySelector(trigger.getAttribute('data-bs-target'));

        if (panel && panel.classList.contains('show')) {
            event.stopPropagation();
        }
    }, true);

    // c) Bring a freshly opened group into view (matters in the scrollable
    //    mobile menu, where the last groups open below the visible area).
    var reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)');

    document.addEventListener('shown.bs.collapse', function (event) {
        var panel = event.target;

        if (!panel.classList.contains('admin-subnav')) {
            return;
        }

        panel.scrollIntoView({
            block: 'nearest',
            behavior: reducedMotion.matches ? 'auto' : 'smooth'
        });
    });
});
