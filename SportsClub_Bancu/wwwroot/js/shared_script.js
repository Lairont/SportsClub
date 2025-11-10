document.addEventListener('DOMContentLoaded', function () {

    window.addEventListener('scroll', function () {
        var header = document.getElementById('main-header');
        var scrollTop = window.scrollY;
        var maxScroll = 250;
        var opacity = Math.min(scrollTop / maxScroll, 1);
        // header.style.backgroundColor = `rgba(255,165,0, ${opacity})`;
    });

    const hamburger = document.getElementById('hamburger');
    const sideMenu = document.getElementById('side-menu');
    const closeButton = document.getElementById('side-menu-button-click-to-hide');

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

    document.addEventListener('click', function (e) {
        if (!sideMenu || !sideMenu.classList.contains('active')) return;

        if (
            e.target === hamburger ||
            e.target === closeButton ||
            e.target.id === 'open-login-side' ||
            sideMenu.contains(e.target)
        ) {
            return;
        }
        sideMenu.classList.remove('active');
    });

});