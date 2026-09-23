/* =========================================================
   PASSWORD VISIBILITY TOGGLE — auth pages
   -----------------------------------------------------------
   Self-contained: no-ops if the page has no .auth-password-toggle
   button. Supports more than one on the same page (e.g. the two
   password fields on Forgot Password's step 3), each wired to
   the input named in its data-target.
========================================================= */

document.addEventListener("DOMContentLoaded", () => {

    document.querySelectorAll(".auth-password-toggle").forEach((btn) => {
        const input = document.getElementById(btn.getAttribute("data-target"));
        const icon = btn.querySelector("i");

        if (!input || !icon) {
            return;
        }

        btn.addEventListener("click", () => {
            const isHidden = input.type === "password";

            input.type = isHidden ? "text" : "password";
            icon.classList.toggle("bi-eye", !isHidden);
            icon.classList.toggle("bi-eye-slash", isHidden);
            btn.setAttribute("aria-pressed", isHidden ? "true" : "false");
            // Labels come from data attributes, not hardcoded here,
            // so this file works unchanged on the English pages too.
            btn.setAttribute("aria-label", isHidden ? btn.dataset.hideLabel : btn.dataset.showLabel);
        });
    });

});
