document.addEventListener('click', function (e) {
    const btn = document.getElementById('userMenuBtn');
    const menu = document.getElementById('userDropdown');

    if (btn && menu) {
        if (btn.contains(e.target)) {
            // Клик по иконке — переключаем видимость
            menu.style.display = (menu.style.display === 'block') ? 'none' : 'block';
        } else if (!menu.contains(e.target)) {
            // Клик мимо меню — закрываем
            menu.style.display = 'none';
        }
    }
});