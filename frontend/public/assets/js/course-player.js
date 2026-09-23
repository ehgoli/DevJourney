/*
 * Course video player.
 * Initializes the video.js player in the course cover area and wires every
 * lesson row's play button to load that lesson's video + update the caption,
 * so it works as a simple "click a lesson, it plays here" experience.
 *
 * Reusable beyond this page: any page with a <video id="coursePlayer"
 * class="video-js"> and a matching set of .course-lesson-item[data-video-src]
 * elements gets this behavior automatically.
 */
document.addEventListener('DOMContentLoaded', function () {
    var videoEl = document.getElementById('coursePlayer');
    if (!videoEl || typeof videojs === 'undefined') {
        return;
    }

    var player = videojs(videoEl, {
        preload: 'metadata',
        playbackRates: [0.75, 1, 1.25, 1.5, 2]
    });

    var captionTextEl = document.getElementById('coursePlayerCaptionText');
    var lessonItems = document.querySelectorAll('.course-lesson-item[data-video-src]');
    var playerWrap = document.querySelector('.course-player-wrap');

    function playLesson(item) {
        var src = item.getAttribute('data-video-src');
        var title = item.getAttribute('data-video-title');
        if (!src) {
            return;
        }

        player.src({ src: src, type: 'video/mp4' });
        player.play();

        if (captionTextEl && title) {
            captionTextEl.textContent = title;
        }

        lessonItems.forEach(function (el) {
            el.classList.remove('is-playing');
        });
        item.classList.add('is-playing');

        if (playerWrap) {
            playerWrap.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }

    lessonItems.forEach(function (item) {
        var trigger = item.querySelector('.course-lesson-play');
        if (trigger) {
            trigger.addEventListener('click', function (e) {
                e.preventDefault();
                playLesson(item);
            });
        }
    });
});
