/*
 * Admin sidebar collapse toggle (desktop only — the mobile sidebar is a
 * Bootstrap offcanvas and needs no JS of its own beyond its own data
 * attributes). Persists the collapsed/expanded state per browser, the
 * same way theme-toggle.js persists the color theme.
 */
document.addEventListener('DOMContentLoaded', function () {
    var shell = document.querySelector('.admin-shell');
    var toggleBtn = document.getElementById('adminSidebarCollapseBtn');
    if (!shell || !toggleBtn) {
        return;
    }

    var STORAGE_KEY = 'adminSidebarCollapsed';

    if (localStorage.getItem(STORAGE_KEY) === '1') {
        shell.classList.add('is-collapsed');
    }

    toggleBtn.addEventListener('click', function () {
        var collapsed = shell.classList.toggle('is-collapsed');
        localStorage.setItem(STORAGE_KEY, collapsed ? '1' : '0');
        toggleBtn.setAttribute('aria-expanded', String(!collapsed));
    });
});
