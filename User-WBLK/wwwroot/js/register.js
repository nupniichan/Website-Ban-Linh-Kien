document.addEventListener('DOMContentLoaded', function() {
    // Toggle password visibility
    document.querySelectorAll('.toggle-password').forEach(function(toggle) {
        toggle.addEventListener('click', function() {
            const input = this.previousElementSibling;
            const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
            input.setAttribute('type', type);
            this.classList.toggle('fa-eye');
            this.classList.toggle('fa-eye-slash');
        });
    });

    // Close register popup
    document.getElementById('closeRegisterBtn').addEventListener('click', function() {
        document.querySelector('.register-container').style.display = 'none';
    });

    // Switch to login
    document.getElementById('switchToLogin').addEventListener('click', function(e) {
        e.preventDefault();
        document.querySelector('.register-container').style.display = 'none';
        // Hiển thị form login (cần thêm logic)
    });

    // Close when clicking outside
    document.querySelector('.register-container').addEventListener('click', function(e) {
        if (e.target === this) {
            this.style.display = 'none';
        }
    });

    // Form submission
    document.querySelector('form').addEventListener('submit', function(e) {
        e.preventDefault();
        // Thêm validation và xử lý form ở đây
        console.log('Form submitted');
    });
});
