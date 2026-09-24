/*
 * تغییر پوسته (روشن/تاریک)
 * -------------------------------------------------------
 * تشخیص و اعمال اولیه‌ی پوسته (برای جلوگیری از چشمک زدن
 * صفحه) در assets/js/theme-init.js انجام می‌شود که در ابتدای
 * <head> هر صفحه لود می‌شود؛ این فایل فقط رفتار خودِ دکمه را
 * وصل می‌کند.
 *
 * ویژگی data-bs-theme روی <html> ست می‌شود چون همان ویژگی
 * استاندارد پوسته تاریک بوت‌استرپ ۵.۳ است (Componentهای
 * خودِ بوت‌استرپ مثل Offcanvas خودکار با آن هماهنگ می‌شوند).
 */

(function () {
    "use strict";

    var STORAGE_KEY = "theme";
    var btn = document.getElementById("themeToggleBtn");

    function currentTheme() {
        return document.documentElement.getAttribute("data-bs-theme") === "dark" ? "dark" : "light";
    }

    function setTheme(theme) {
        document.documentElement.setAttribute("data-bs-theme", theme);

        try {
            localStorage.setItem(STORAGE_KEY, theme);
        } catch (e) {
            /* localStorage در دسترس نیست (حالت خصوصی و ...)؛ مشکلی نیست */
        }

        if (btn) {
            btn.setAttribute("aria-pressed", theme === "dark" ? "true" : "false");
        }

        // به بخش‌های دیگر صفحه (مثل ذرات هیرو) اطلاع بده که پوسته عوض شد
        document.dispatchEvent(new CustomEvent("themechange", { detail: { theme: theme } }));
    }

    if (btn) {
        btn.setAttribute("aria-pressed", currentTheme() === "dark" ? "true" : "false");

        btn.addEventListener("click", function () {
            setTheme(currentTheme() === "dark" ? "light" : "dark");
        });
    }

    // اگر کاربر در تب دیگری از همین سایت پوسته را عوض کند، این تب هم هماهنگ شود
    window.addEventListener("storage", function (e) {
        if (e.key === STORAGE_KEY && e.newValue) {
            setTheme(e.newValue);
        }
    });
})();
