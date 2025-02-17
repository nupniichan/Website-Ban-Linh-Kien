// Regex cho email
const emailRegex = /^([\w.%+-]+)@([\w-]+\.)+([\w]{2,})$/i;

// Validate form
function validateCustomerForm(e) {
    e.preventDefault();
    
    // Lấy các giá trị
    const hoten = document.getElementById('Hoten').value.trim();
    const email = document.getElementById('Email').value.trim();
    const sodienthoai = document.getElementById('Sodienthoai').value.trim();
    const diachi = document.getElementById('Diachi').value.trim();
    const ngaysinh = document.getElementById('Ngaysinh').value;
    const gioitinh = document.getElementById('Gioitinh')?.value;

    let isValid = true;
    let errorMessages = [];

    // Validate họ tên
    if (hoten.length < 2 || hoten.length > 50) {
        isValid = false;
        errorMessages.push('Họ tên phải từ 2 đến 50 ký tự');
        document.getElementById('Hoten').classList.add('border-red-500');
    }

    // Validate email
    if (!emailRegex.test(email)) {
        isValid = false;
        errorMessages.push('Email không hợp lệ');
        document.getElementById('Email').classList.add('border-red-500');
    }

    // Validate số điện thoại
    if (!/^0[0-9]{9}$/.test(sodienthoai)) {
        isValid = false;
        errorMessages.push('Số điện thoại không hợp lệ (phải bắt đầu bằng 0 và có 10 số)');
        document.getElementById('Sodienthoai').classList.add('border-red-500');
    }

    // Validate địa chỉ
    const diachiParts = diachi.split(',');
    if (diachi.length > 100) {
        isValid = false;
        errorMessages.push('Địa chỉ không được vượt quá 100 ký tự');
        document.getElementById('Diachi').classList.add('border-red-500');
        document.getElementById('diachiError').classList.remove('hidden');
    }
    else if (diachiParts.length !== 4) {
        isValid = false;
        errorMessages.push('Địa chỉ phải có đủ 4 phần: Số nhà/Tên đường, Phường/Xã/Thị trấn, Quận/Huyện, Tỉnh/Thành phố');
        document.getElementById('Diachi').classList.add('border-red-500');
        document.getElementById('diachiError').classList.remove('hidden');
    } else {
        // Kiểm tra từng phần không được để trống
        for (let part of diachiParts) {
            if (part.trim() === '') {
                isValid = false;
                errorMessages.push('Các thành phần của địa chỉ không được để trống');
                document.getElementById('Diachi').classList.add('border-red-500');
                document.getElementById('diachiError').classList.remove('hidden');
                break;
            }
        }
    }

    // Validate giới tính (chỉ cho form Create)
    if (gioitinh !== undefined && !gioitinh) {
        isValid = false;
        errorMessages.push('Vui lòng chọn giới tính');
        document.getElementById('Gioitinh').classList.add('border-red-500');
    }

    // Validate ngày sinh
    const birthDate = new Date(ngaysinh);
    const today = new Date();
    const ngaysinhError = document.getElementById('NgaysinhError');

    // Kiểm tra xem có chọn ngày không
    if (!ngaysinh) {
        isValid = false;
        ngaysinhError.textContent = 'Vui lòng chọn ngày sinh';
        document.getElementById('Ngaysinh').classList.add('border-red-500');
    }
    else {
        // Tính tuổi chính xác
        let age = today.getFullYear() - birthDate.getFullYear();
        const monthDiff = today.getMonth() - birthDate.getMonth();
        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }

        // Kiểm tra các điều kiện
        if (birthDate > today) {
            isValid = false;
            ngaysinhError.textContent = 'Ngày sinh không thể là ngày trong tương lai';
            document.getElementById('Ngaysinh').classList.add('border-red-500');
        }
        else if (age < 18) {
            isValid = false;
            ngaysinhError.textContent = 'Bạn phải đủ 18 tuổi';
            document.getElementById('Ngaysinh').classList.add('border-red-500');
        }
        else if (age > 100) {
            isValid = false;
            ngaysinhError.textContent = 'Tuổi không được quá 100';
            document.getElementById('Ngaysinh').classList.add('border-red-500');
        }
    }

    // Hiển thị lỗi nếu có
    const validationSummary = document.getElementById('validation-summary');
    if (!isValid) {
        validationSummary.innerHTML = errorMessages.map(msg => `<li>${msg}</li>`).join('');
        validationSummary.classList.remove('hidden');
        return;
    }

    // Nếu không có lỗi, submit form
    this.submit();
}

// Reset validation
function resetValidation() {
    document.querySelectorAll('input, select').forEach(input => {
        input.addEventListener('input', function() {
            this.classList.remove('border-red-500');
            if (this.id === 'Ngaysinh') {
                document.getElementById('NgaysinhError').textContent = '';
            }
            if (this.id === 'Diachi') {
                document.getElementById('diachiError').classList.add('hidden');
            }
            document.getElementById('validation-summary').innerHTML = '';
        });
    });
}

// Initialize validation
document.addEventListener('DOMContentLoaded', function() {
    const form = document.querySelector('form');
    if (form) {
        form.addEventListener('submit', validateCustomerForm);
        resetValidation();
    }
}); 