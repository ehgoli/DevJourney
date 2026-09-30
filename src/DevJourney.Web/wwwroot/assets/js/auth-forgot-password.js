/* =========================================================
   FORGOT PASSWORD
   -----------------------------------------------------------
   Server-side flow:
   - Step transitions are handled by Razor Pages.
   - Form submissions are NOT intercepted.
   - Backend validates phone, OTP, and reset token.

   Client-side responsibilities:
   - Mask the phone number on the verification step.
   - Manage the resend countdown for UX only.

   Security-sensitive operations must always be enforced
   on the server.
========================================================= */

document.addEventListener("DOMContentLoaded", () => {

    const panels = document.querySelectorAll(".auth-step-panel");
    if (!panels.length) {
        return;
    }

    let resendIntervalId = null;

    /* ---------------------------------------------------------
       Phone masking
    --------------------------------------------------------- */

    const maskPhone = (raw) => {
        const digits = raw.replace(/\D/g, "");

        if (digits.length <= 6) {
            return raw;
        }

        const head = digits.slice(0, 4);
        const tail = digits.slice(-3);
        const middle = "*".repeat(
            Math.max(digits.length - 7, 3)
        );

        return `${head}${middle}${tail}`;
    };

    const updateMaskedPhone = () => {
        const phoneInput = document.getElementById("forgotPhone");
        const maskedElement =
            document.getElementById("forgotPhoneMasked");

        if (!phoneInput || !maskedElement) {
            return;
        }

        maskedElement.textContent =
            maskPhone(phoneInput.value);
    };

    updateMaskedPhone();

    /* ---------------------------------------------------------
       Resend countdown
       ---------------------------------------------------------
       This timer is for UX only.
       The actual 60-second restriction is enforced
       by the backend.
    --------------------------------------------------------- */

    const startResendTimer = (seconds) => {
        const button =
            document.getElementById("resendCodeBtn");

        const timer =
            document.getElementById("resendTimer");

        if (!button || !timer) {
            return;
        }

        clearInterval(resendIntervalId);

        let remaining = seconds;

        button.disabled = true;

        const tick = () => {
            const minutes = String(
                Math.floor(remaining / 60)
            ).padStart(2, "0");

            const seconds = String(
                remaining % 60
            ).padStart(2, "0");

            timer.textContent = `(${minutes}:${seconds})`;

            if (remaining <= 0) {
                clearInterval(resendIntervalId);

                button.disabled = false;
                timer.textContent = "";

                return;
            }

            remaining -= 1;
        };

        tick();

        resendIntervalId = setInterval(
            tick,
            1000
        );
    };

    /* ---------------------------------------------------------
       Start timer when verification step is displayed.
       A successful request or resend creates a new 60-second
       cooldown on the server.
    --------------------------------------------------------- */

    const verificationPanel =
        document.querySelector(
            '.auth-step-panel[data-step="2"]'
        );

    if (
        verificationPanel &&
        !verificationPanel.classList.contains("d-none")
    ) {
        startResendTimer(60);
    }
});