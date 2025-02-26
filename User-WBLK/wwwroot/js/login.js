document.addEventListener('DOMContentLoaded', function() {
    const loginPopup = document.getElementById('loginPopup');
    const closeLoginBtn = document.getElementById('closeLoginBtn');
    const togglePassword = document.getElementById('togglePassword');
    const passwordInput = document.querySelector('input[name="password"]');
    const loginForm = document.getElementById('loginForm');
    const errorMessage = document.querySelector('.error-message');

    // Mở login popup
    document.querySelectorAll('a[href="/Login"]').forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            loginPopup.classList.remove('hidden');
        });
    });

    // Đóng login popup
    closeLoginBtn.addEventListener('click', function() {
        loginPopup.classList.add('hidden');
        errorMessage.classList.add('hidden');
        loginForm.reset();
    });

    // Đóng khi click ra ngoài
    loginPopup.addEventListener('click', function(e) {
        if (e.target === loginPopup) {
            loginPopup.classList.add('hidden');
            errorMessage.classList.add('hidden');
            loginForm.reset();
        }
    });

    // Toggle password visibility
    togglePassword.addEventListener('click', function() {
        const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordInput.setAttribute('type', type);
        
        // Toggle giữa hai icon
        const eyeOpen = this.querySelector('.eye-open');
        const eyeClosed = this.querySelector('.eye-closed');
        eyeOpen.classList.toggle('hidden');
        eyeClosed.classList.toggle('hidden');
    });

    // Xử lý form submit với Ajax
    if (loginForm) {
        loginForm.addEventListener('submit', function(e) {
            e.preventDefault();
            const formData = new FormData(this);

            fetch('/Login/Login', {
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
    }

    // Chuyển sang form đăng ký
    document.getElementById('showRegisterBtn').addEventListener('click', function(e) {
        e.preventDefault();
        document.getElementById('loginPopup').classList.add('hidden');
        document.getElementById('registerPopup').classList.remove('hidden');
    });

    // Tương tự cho các phần tử khác
    const registerForm = document.getElementById('registerForm');
    if (registerForm) {
        registerForm.addEventListener('submit', function(event) {
            // Xử lý form đăng ký
        });
    }
});