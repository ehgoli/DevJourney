/* =========================================================
   CERTIFICATE LIGHTBOX — کامپوننت عمومی
   -----------------------------------------------------------
   این فایل مستقل از صفحه است: به هر Modal با id="certificateModal"
   که با یک دکمه‌ی certificate-thumb (دارای data-cert-image و
   data-cert-title) باز شود، واکنش نشان می‌دهد و تصویر/عنوان را
   داخل Modal قرار می‌دهد. برای افزودن گواهینامه‌ی جدید در هر
   صفحه‌ای، کافی است یک دکمه‌ی مشابه با همان دو data-attribute
   اضافه کنی؛ نیازی به تغییر این فایل نیست.
========================================================= */

document.addEventListener("DOMContentLoaded", () => {

    const certificateModal = document.getElementById("certificateModal");

    if (!certificateModal) {
        return;
    }

    const modalImage = document.getElementById("certificateModalImage");
    const modalCaption = document.getElementById("certificateModalCaption");

    certificateModal.addEventListener("show.bs.modal", (event) => {
        const trigger = event.relatedTarget;

        if (!trigger) {
            return;
        }

        const imageSrc = trigger.getAttribute("data-cert-image") || "";
        const title = trigger.getAttribute("data-cert-title") || "";

        modalImage.setAttribute("src", imageSrc);
        modalImage.setAttribute("alt", title);
        modalCaption.textContent = title;
    });

    // پاک کردن src هنگام بسته شدن، تا عکس قبلی برای یک لحظه هنگام باز شدن Modal بعدی دیده نشود
    certificateModal.addEventListener("hidden.bs.modal", () => {
        modalImage.setAttribute("src", "");
        modalImage.setAttribute("alt", "");
        modalCaption.textContent = "";
    });

});
