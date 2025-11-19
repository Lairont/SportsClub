function toggleLoginContainer() {
    const container = document.querySelector(".container-login-registration");
    if (!container) return;

    if (container.style.display === "grid") {
        container.style.display = "none";
    } else {
        container.style.display = "grid";
    }
}

function sendRequest(method, url, body = null) {
    const headers = {
        'Content-Type': 'application/json'
    };

    return fetch(url, {
        method: method,
        body: JSON.stringify(body),
        headers: headers
    }).then(response => {
        if (!response.ok) {
          
            return response.json().then(errorData => {
                throw errorData;
            });
        }
        return response.json();
    });
}

function displayErrors(errors, errorContainer) {
    errorContainer.innerHTML = '';

    if (!Array.isArray(errors)) {
        const arr = [];
        for (const key in errors) {
            if (Array.isArray(errors[key])) {
                arr.push(...errors[key]);
            } else {
                arr.push(errors[key]);
            }
        }
        errors = arr;
    }

    errors.forEach(error => {
        const errorMessage = document.createElement('div');
        errorMessage.classList.add('error');
        errorMessage.textContent = error;
        errorContainer.appendChild(errorMessage);
    });
}

function cleaningAndClosingForm(form, errorContainer) {
    errorContainer.innerHTML = '';

    for (const key in form) {
        if (form.hasOwnProperty(key)) {
            if (form[key] && form[key].value !== undefined) {
                form[key].value = '';
            }
        }
    }

    toggleLoginContainer();
}

// поддержка ОБЕИХ кнопок "Войти"
document.addEventListener('DOMContentLoaded', function () {
    const overlay = document.querySelector(".overlay");

    const openLoginBtn = document.getElementById("open-login");
    const openLoginSideBtn = document.getElementById("open-login-side");

    if (openLoginBtn) openLoginBtn.addEventListener("click", toggleLoginContainer);
    if (openLoginSideBtn) openLoginSideBtn.addEventListener("click", toggleLoginContainer);

    if (overlay) overlay.addEventListener("click", toggleLoginContainer);

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

// ----------------------------
// Кнопка входа
// ----------------------------
const form_btn_signin = document.querySelector('.form_btn_signin');

if (form_btn_signin) {
    form_btn_signin.addEventListener('click', function () {
        const requestURL = '/Home/Login';
        const errorContainer = document.getElementById('error-messages-singin');

        const form = {
            email: document.getElementById("signin_email"),
            password: document.getElementById("signin_password")
        };

        const body = {
            email: form.email.value,
            password: form.password.value
        };

        sendRequest('POST', requestURL, body)
            .then(data => {
                cleaningAndClosingForm(form, errorContainer);
                console.log('Успешный вход:', data);
                location.reload();
            })
            .catch(err => {
                displayErrors(err, errorContainer);
                console.log('Ошибка входа:', err);
            });
    });
}

// ----------------------------
// Кнопка регистрации
// ----------------------------
const form_btn_signup = document.querySelector('.form_btn_signup');

if (form_btn_signup) {
    form_btn_signup.addEventListener('click', function () {
        const requestURL = '/Home/Register';
        const errorContainer = document.getElementById('error-messages-singup');

        const form = {
            Login: document.getElementById("signup_login"),
            Email: document.getElementById("signup_email"),
            Password: document.getElementById("signup_password"),
            PasswordConfirm: document.getElementById("signup_confirm_password")
        };

        const body = {
            Login: form.Login.value,
            Email: form.Email.value,
            Password: form.Password.value,
            PasswordReset: form.PasswordConfirm.value 
        };

        sendRequest('POST', requestURL, body)
            .then(data => {
                cleaningAndClosingForm(form, errorContainer);
                console.log('Успешная регистрация:', data);
                location.reload();
            })
            .catch(err => {
                displayErrors(err, errorContainer);
                console.log('Ошибка регистрации:', err);
            });
    });
}