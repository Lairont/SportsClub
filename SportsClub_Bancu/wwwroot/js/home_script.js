document.addEventListener('DOMContentLoaded', function () {
    const cardWrapper = document.querySelector('.card-wrapper');
    const cards = Array.from(document.querySelectorAll('.card'));
    const cardsPerRow = 3;

    if (!cards.length) return;

    // Клонируем карточки в начале и конце для бесконечной прокрутки
    const prependCards = cards.slice(-cardsPerRow).map(c => c.cloneNode(true));
    const appendCards = cards.slice(0, cardsPerRow).map(c => c.cloneNode(true));

    prependCards.forEach(c => cardWrapper.insertBefore(c, cardWrapper.firstChild));
    appendCards.forEach(c => cardWrapper.appendChild(c));

    let currentIndex = cardsPerRow; // начинаем с «настоящих» карточек
    const totalCards = cardWrapper.children.length;

    // ширина карточки + gap
    const cardWidth = cards[0].offsetWidth + 24; // 24 — это gap между карточками

    function updateTransform(animate = true) {
        if (animate) {
            cardWrapper.style.transition = 'transform 0.5s ease';
        } else {
            cardWrapper.style.transition = 'none';
        }
        cardWrapper.style.transform = `translateX(-${currentIndex * cardWidth}px)`;
    }

    function next() {
        currentIndex++;
        updateTransform();
        if (currentIndex === totalCards - cardsPerRow) {
            // после анимации прыгаем обратно к «настоящим»
            setTimeout(() => {
                currentIndex = cardsPerRow;
                updateTransform(false);
            }, 500);
        }
    }

    function prev() {
        currentIndex--;
        updateTransform();
        if (currentIndex === 0) {
            setTimeout(() => {
                currentIndex = totalCards - (2 * cardsPerRow);
                updateTransform(false);
            }, 500);
        }
    }

    const arrowRight = document.querySelector('.arrow.right');
    const arrowLeft = document.querySelector('.arrow.left');

    if (arrowRight) arrowRight.addEventListener('click', next);
    if (arrowLeft) arrowLeft.addEventListener('click', prev);

    // стартовое положение
    updateTransform(false);
});
