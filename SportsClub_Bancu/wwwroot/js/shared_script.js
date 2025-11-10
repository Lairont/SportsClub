document.addEventListener('DOMContentLoaded', function () {

    // Эффект прокрутки для шапки
    window.addEventListener('scroll', function () {
        var header = document.getElementById('main-header');
        var scrollTop = window.scrollY;
        var maxScroll = 250;
        var opacity = Math.min(scrollTop / maxScroll, 1);
        // header.style.backgroundColor = `rgba(255,165,0, ${opacity})`;
    });

    // Элементы меню
    const hamburger = document.getElementById('hamburger');
    const sideMenu = document.getElementById('side-menu');
    const closeButton = document.getElementById('side-menu-button-click-to-hide');

    // Открытие/закрытие по кнопкам
    if (hamburger && sideMenu) {
        hamburger.addEventListener('click', function () {
            sideMenu.classList.toggle('active');
        });
    }

    if (closeButton && sideMenu) {
        closeButton.addEventListener('click', function () {
            sideMenu.classList.remove('active');
        });
    }

    // 🔥 Закрытие по клику вне меню
    document.addEventListener('click', function (e) {
        // Пропускаем, если меню не открыто
        if (!sideMenu || !sideMenu.classList.contains('active')) return;

        // Пропускаем клики по: гамбургеру, кнопке закрытия, кнопке "Войти", и любому содержимому меню
        if (
            e.target === hamburger ||
            e.target === closeButton ||
            e.target.id === 'open-login-side' ||
            sideMenu.contains(e.target)
        ) {
            return;
        }

        // Закрываем меню
        sideMenu.classList.remove('active');
    });

});