document.addEventListener('DOMContentLoaded', function () {
    const registerForm = document.getElementById('registerForm');

    // Form validation
    registerForm.addEventListener('submit', function (e) {
        e.preventDefault();
        if (validateRegisterForm()) {
            console.log('Form is valid, proceeding with registration...');
            const formData = new FormData(registerForm);
            const data = Object.fromEntries(formData);
            console.log('Registration data:', data);

            // Add your registration logic here
        }
    });

    // Real-time validation
    const inputs = registerForm.querySelectorAll('input[required]');
    inputs.forEach(input => {
        input.addEventListener('input', () => {
            validateInput(input);
            if (input.id === 'confirm-password' || input.id === 'password') {
                validatePasswordMatch();
            }
        });
    });
});

function validateRegisterForm() {
    const fullname = document.getElementById('fullname');
    const phone = document.getElementById('phone');
    const email = document.getElementById('email');
    const password = document.getElementById('password');
    const confirmPassword = document.getElementById('confirm-password');
    let isValid = true;

    if (!validateInput(fullname)) isValid = false;
    if (!validateInput(phone)) isValid = false;
    if (!validateInput(email)) isValid = false;
    if (!validateInput(password)) isValid = false;
    if (!validateInput(confirmPassword)) isValid = false;
    if (!validatePasswordMatch()) isValid = false;

    return isValid;
}

function validateInput(input) {
    const errorElement = input.nextElementSibling;
    let errorMessage = '';

    input.classList.remove('error');

    if (!input.value.trim()) {
        errorMessage = 'Trường này không được để trống';
    }
    else {
        switch (input.id) {
            case 'fullname':
                if (input.value.length < 2) {
                    errorMessage = 'Tên phải có ít nhất 2 ký tự';
                }
                break;
            case 'phone':
                if (!validatePhone(input.value)) {
                    errorMessage = 'Số điện thoại không hợp lệ';
                }
                break;
            case 'email':
                if (!validateEmail(input.value)) {
                    errorMessage = 'Email không hợp lệ';
                }
                break;
            case 'password':
                if (!validatePassword(input.value)) {
                    errorMessage = 'Mật khẩu phải có ít nhất 6 ký tự, bao gồm chữ và số';
                }
                break;
        }
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

function validatePasswordMatch() {
    const password = document.getElementById('password');
    const confirmPassword = document.getElementById('confirm-password');
    const errorElement = confirmPassword.nextElementSibling;

    if (password.value !== confirmPassword.value) {
        confirmPassword.classList.add('error');
        errorElement.textContent = 'Mật khẩu không khớp';
        return false;
    }
    return true;
}

function validateEmail(email) {
    const re = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return re.test(String(email).toLowerCase());
}

function validatePhone(phone) {
    const re = /(84|0[3|5|7|8|9])+([0-9]{8})\b/;
    return re.test(phone);
}

function validatePassword(password) {
    const re = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;
    return re.test(password);
} 