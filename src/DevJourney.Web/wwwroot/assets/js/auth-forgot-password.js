/* =========================================================
   FORGOT PASSWORD — step wizard
   -----------------------------------------------------------
   Three panels (phone -> code -> new password) plus a final
   "done" panel, all inside one card. No backend yet: each
   form's submit is intercepted and just advances to the next
   panel after its own required fields pass native validation.
   Swap the advance calls below for real API calls once the
   backend exists — the phone lookup and code check are not
   actually verified against anything yet.

   Self-contained: no-ops if the page has no .auth-step-panel.
========================================================= */

document.addEventListener("DOMContentLoaded", () => {

    const panels = document.querySelectorAll(".auth-step-panel");
    if (!panels.length) {
        return;
    }

    const dots = document.querySelectorAll(".auth-step");
    const stepsWrap = document.getElementById("authSteps");

    const showPanel = (key) => {
        panels.forEach((panel) => {
            panel.classList.toggle("d-none", panel.dataset.step !== key);
        });

        if (key === "done") {
            if (stepsWrap) {
                stepsWrap.classList.add("d-none");
            }
            return;
        }

        const stepNumber = Number(key);
        dots.forEach((dot, index) => {
            dot.classList.toggle("is-active", index < stepNumber);
        });
    };

    // Iranian mobile numbers only, matching the placeholder/type used
    // on the phone field — good enough for a static prototype.
    const maskPhone = (raw) => {
        const digits = raw.replace(/\D/g, "");
        if (digits.length <= 6) {
            return raw;
        }
        const head = digits.slice(0, 4);
        const tail = digits.slice(-3);
        const middle = "*".repeat(Math.max(digits.length - 7, 3));
        return `${head}${middle}${tail}`;
    };

    let resendIntervalId = null;

    const startResendTimer = (seconds) => {
        const btn = document.getElementById("resendCodeBtn");
        const timer = document.getElementById("resendTimer");
        if (!btn || !timer) {
            return;
        }

        clearInterval(resendIntervalId);
        let remaining = seconds;
        btn.disabled = true;

        const tick = () => {
            const mm = String(Math.floor(remaining / 60)).padStart(2, "0");
            const ss = String(remaining % 60).padStart(2, "0");
            timer.textContent = `(${mm}:${ss})`;

            if (remaining <= 0) {
                clearInterval(resendIntervalId);
                btn.disabled = false;
                timer.textContent = "";
                return;
            }
            remaining -= 1;
        };

        tick();
        resendIntervalId = setInterval(tick, 1000);
    };

    // Step 1 -> Step 2
    const step1Form = document.getElementById("forgotStep1Form");
    if (step1Form) {
        step1Form.addEventListener("submit", (e) => {
            e.preventDefault();
            const phoneInput = document.getElementById("forgotPhone");
            const maskedEl = document.getElementById("forgotPhoneMasked");
            if (phoneInput && maskedEl) {
                maskedEl.textContent = maskPhone(phoneInput.value);
            }
            showPanel("2");
            startResendTimer(60);
        });
    }

    // Resend code
    const resendBtn = document.getElementById("resendCodeBtn");
    if (resendBtn) {
        resendBtn.addEventListener("click", () => startResendTimer(60));
    }

    // Back to step 1 (e.g. the phone number was mistyped)
    const backToPhoneBtn = document.getElementById("backToPhoneBtn");
    if (backToPhoneBtn) {
        backToPhoneBtn.addEventListener("click", () => {
            clearInterval(resendIntervalId);
            showPanel("1");
        });
    }

    // Step 2 -> Step 3
    const step2Form = document.getElementById("forgotStep2Form");
    if (step2Form) {
        step2Form.addEventListener("submit", (e) => {
            e.preventDefault();
            clearInterval(resendIntervalId);
            showPanel("3");
        });
    }

    // Step 3 -> Done (this one check IS real: it only compares the
    // two fields on this page, no backend needed for that part)
    const step3Form = document.getElementById("forgotStep3Form");
    if (step3Form) {
        step3Form.addEventListener("submit", (e) => {
            e.preventDefault();
            const pass = document.getElementById("forgotNewPassword").value;
            const confirm = document.getElementById("forgotConfirmPassword").value;
            const alertEl = document.getElementById("forgotAlert3");

            if (pass !== confirm) {
                if (alertEl) {
                    alertEl.classList.remove("d-none");
                }
                return;
            }
            if (alertEl) {
                alertEl.classList.add("d-none");
            }
            showPanel("done");
        });
    }

});
