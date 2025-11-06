document.addEventListener('DOMContentLoaded', function () {

    // Эффект прокрутки для шапки
    window.addEventListener('scroll', function () {
        var header = document.getElementById('header-top');
        var scrollTop = window.scrollY;
        var maxScroll = 250;

        var opacity = Math.min(scrollTop / maxScroll, 1);
        header.style.backgroundColor = `rgba(255,165,0, ${opacity})`;
    });

    // Обработчик меню
    const hamburger = document.getElementById('hamburger');
    const sideMenu = document.getElementById('side-menu');

    if (hamburger && sideMenu) {
        hamburger.addEventListener('click', function () {
            sideMenu.classList.toggle('active');
        });
    }
});
