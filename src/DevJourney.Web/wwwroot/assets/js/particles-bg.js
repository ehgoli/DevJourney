/* =========================================================
   PARTICLES BACKGROUND — کامپوننت عمومی و مستقل از صفحه
   -----------------------------------------------------------
   این اسکریپت به هیچ صفحه یا بخش خاصی وابسته نیست: فقط دنبال
   عنصری با id="particles-bg" می‌گردد و اگر پیدا کند، پس‌زمینه‌ی
   ذرات (tsParticles) را داخلش می‌سازد؛ اگر پیدا نکند، بی‌صدا
   خارج می‌شود. برای استفاده در یک صفحه‌ی جدید، کافی است یک
   div با همین id داخل هر بخشی (نه فقط هیرو) قرار بدهی و همین
   دو فایل (particles-bg.css/js) را لود کنی — نیازی به هیچ
   تغییر دیگری در این فایل نیست.
========================================================= */

document.addEventListener("DOMContentLoaded", async () => {

    const particlesElement = document.getElementById("particles-bg");

    if (!particlesElement) {
        return;
    }

    await loadSlim(tsParticles);

    // رنگ ذرات باید با پوسته فعلی هماهنگ باشد؛ در حالت تاریک
    // رنگ‌های تیره‌ی اصلی (#374151/#6b7280) روی پس‌زمینه‌ی تیره
    // تقریباً دیده نمی‌شوند، پس یک نسخه‌ی روشن‌تر استفاده می‌شود.
    function isDarkTheme() {
        return document.documentElement.getAttribute("data-bs-theme") === "dark";
    }

    function particleColors() {
        return isDarkTheme()
            ? { particle: "#8ba1bd", link: "#9aa8bd" }
            : { particle: "#374151", link: "#6b7280" };
    }

    async function loadParticles() {
        const colors = particleColors();

        await tsParticles.load({
            id: "particles-bg",

            options: {

                fullScreen: {
                    enable: false
                },

                background: {
                    color: {
                        value: "transparent"
                    }
                },

                particles: {

                    number: {
                        value: 100,

                        density: {
                            enable: true,
                            area: 700
                        }
                    },

                    color: {
                        value: colors.particle
                    },

                    opacity: {
                        value: 0.5
                    },

                    size: {
                        value: {
                            min: 1,
                            max: 2.8
                        }
                    },

                    links: {
                        enable: true,

                        distance: 155,

                        color: colors.link,

                        opacity: 0.34,

                        width: 1
                    },

                    move: {
                        enable: true,

                        speed: 1.05,

                        direction: "none",

                        random: true,

                        straight: false,

                        outModes: {
                            default: "bounce"
                        }
                    }
                },

                detectRetina: true
            }
        });
    }

    await loadParticles();

    // وقتی کاربر پوسته را با دکمه تغییر می‌دهد، ذرات با رنگ جدید بازسازی شوند
    document.addEventListener("themechange", loadParticles);

});
