document.addEventListener('DOMContentLoaded', function () {
    const container = document.querySelector(".container-login-registration");
    const overlay = document.querySelector(".overlay");
    const openBtn = document.getElementById("open-login");

    function toggleLoginContainer() {
        if (container.style.display === "grid") {
            container.style.display = "none";
        } else {
            container.style.display = "grid";
        }
    }

    // открытие / закрытие окна
    if (openBtn) openBtn.addEventListener("click", toggleLoginContainer);
    if (overlay) overlay.addEventListener("click", toggleLoginContainer);

    // переключение форм
    const signInBtn = document.querySelector('.signin-btn');
    const signUpBtn = document.querySelector('.signup-btn');
    const formBox = document.querySelector('.form-box');
    const block = document.querySelector('.block');

    if (signInBtn && signUpBtn && formBox && block) {
        signUpBtn.addEventListener('click', function () {
            formBox.classList.add('active');
            block.classList.add('active');
        });

        signInBtn.addEventListener('click', function () {
            formBox.classList.remove('active');
            block.classList.remove('active');
        });
    }
});
