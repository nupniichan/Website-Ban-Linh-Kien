document.addEventListener('DOMContentLoaded', function() {
    const registerPopup = document.getElementById('registerPopup');
    const closeRegisterBtn = document.getElementById('closeRegisterBtn');
    const registerForm = document.getElementById('registerForm');
    const errorMessage = registerForm.querySelector('.error-message');

    // Mở register popup từ nút đăng ký trong login form
    document.getElementById('showRegisterBtn').addEventListener('click', function(e) {
        e.preventDefault();
        document.getElementById('loginPopup').classList.add('hidden');
        registerPopup.classList.remove('hidden');
    });

    // Đóng register popup
    closeRegisterBtn.addEventListener('click', function() {
        registerPopup.classList.add('hidden');
        errorMessage.classList.add('hidden');
        registerForm.reset();
    });

    // Đóng khi click ra ngoài
    registerPopup.addEventListener('click', function(e) {
        if (e.target === registerPopup) {
            registerPopup.classList.add('hidden');
            errorMessage.classList.add('hidden');
            registerForm.reset();
        }
    });

    // Toggle password visibility
    document.querySelectorAll('.toggle-password').forEach(function(toggle) {
        toggle.addEventListener('click', function() {
            const input = this.parentElement.querySelector('input');
            const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
            input.setAttribute('type', type);
            this.querySelector('i').classList.toggle('fa-eye');
            this.querySelector('i').classList.toggle('fa-eye-slash');
        });
    });

    // Switch to login
    document.getElementById('switchToLogin').addEventListener('click', function(e) {
        e.preventDefault();
        registerPopup.classList.add('hidden');
        document.getElementById('loginPopup').classList.remove('hidden');
    });

    // Prevent any default form submissions
    registerForm.addEventListener('submit', function(e) {
        e.preventDefault();
        e.stopPropagation();
        
        const formData = new FormData(this);

        fetch('/Register/Register', {
            method: 'POST',
            body: formData
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                window.location.href = data.redirectUrl;
            } else {
                errorMessage.textContent = data.message;
                errorMessage.classList.remove('hidden');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            errorMessage.textContent = 'Đã có lỗi xảy ra. Vui lòng thử lại.';
            errorMessage.classList.remove('hidden');
        });
    });

    // Handle select change without form submission
    const genderSelect = registerForm.querySelector('select[name="gender"]');
    genderSelect.addEventListener('change', function(e) {
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();
    });

    // Prevent any form submission on select interaction
    genderSelect.addEventListener('click', function(e) {
        e.stopPropagation();
    });
});