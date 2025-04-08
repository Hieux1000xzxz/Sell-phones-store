document.addEventListener('DOMContentLoaded', function () {
    const loginForm = document.getElementById('loginForm');

    // Form validation
    loginForm.addEventListener('submit', function (e) {
        e.preventDefault();

        if (validateLoginForm()) {
            console.log('Form is valid, proceeding with login...');
            const formData = new FormData(loginForm);
            const data = Object.fromEntries(formData);
            console.log('Login data:', data);

            // Add your login logic here
        }
    });

    // Real-time validation
    const inputs = loginForm.querySelectorAll('input[required]');
    inputs.forEach(input => {
        input.addEventListener('input', () => {
            validateInput(input);
        });
    });
});

function validateLoginForm() {
    const email = document.getElementById('email');
    const password = document.getElementById('password');
    let isValid = true;

    if (!validateInput(email)) isValid = false;
    if (!validateInput(password)) isValid = false;

    return isValid;
}

function validateInput(input) {
    const errorElement = input.nextElementSibling;
    let errorMessage = '';

    input.classList.remove('error');

    if (!input.value.trim()) {
        errorMessage = 'Trường này không được để trống';
    }
    else if (input.type === 'email' && !validateEmail(input.value)) {
        errorMessage = 'Email không hợp lệ';
    }
    else if (input.type === 'password' && input.value.length < 6) {
        errorMessage = 'Mật khẩu phải có ít nhất 6 ký tự';
    }

    if (errorMessage) {
        input.classList.add('error');
        errorElement.textContent = errorMessage;
        return false;
    } else {
        errorElement.textContent = '';
        return true;
    }
}

function validateEmail(email) {
    const re = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return re.test(String(email).toLowerCase());
} 