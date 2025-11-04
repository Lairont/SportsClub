document.addEventListener('DOMContentLoaded', function () {
    let currentSlide = 0;
    const cardWrapper = document.querySelector('.card-wrapper');
    const cards = document.querySelectorAll('.card');
    const cardsPerRow = 3;

    if (!cards.length) return;

    const totalSlides = Math.ceil(cards.length / cardsPerRow);


    function updateTransform() {
        // Используем transform с процентами — 0%, 100%, 200% и т.д.
        cardWrapper.style.transform = `translateX(-${currentSlide * 100}%)`;
    }

    function next() {
        currentSlide = (currentSlide + 1) % totalSlides;
        updateTransform();
    }

    function prev() {
        currentSlide = (currentSlide - 1 + totalSlides) % totalSlides;
        updateTransform();
    }

    const arrowRight = document.querySelector('.arrow.right');
    const arrowLeft = document.querySelector('.arrow.left');

    if (arrowRight) arrowRight.addEventListener('click', next);
    if (arrowLeft) arrowLeft.addEventListener('click', prev);

    updateTransform();
});