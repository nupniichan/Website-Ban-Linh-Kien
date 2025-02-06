// Constants
const MIN_PRICE = 1000; // Giá tối thiểu 1.000 VNĐ
const MAX_PRICE = 1000000000; // Giá tối đa 1 tỷ VNĐ
const MAX_QUANTITY = 10000; // Số lượng tối đa
const MIN_NAME_LENGTH = 3; // Độ dài tối thiểu tên sản phẩm
const MAX_NAME_LENGTH = 200; // Độ dài tối đa tên sản phẩm
const MIN_BRAND_LENGTH = 2; // Độ dài tối thiểu thương hiệu
const MAX_BRAND_LENGTH = 50; // Độ dài tối đa thương hiệu

// Initialize form handling
document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('productForm');
    if (form) {
        form.addEventListener('submit', function(e) {
            // Ngăn form submit mặc định và ngăn chặn lan truyền sự kiện
            e.preventDefault();
            e.stopPropagation();

            // Thực hiện validation
            if (validateProductForm()) {
                // Cập nhật giá trị mô tả
                const moTaEditor = document.getElementById('moTaEditor');
                const moTaInput = document.querySelector('textarea[name="Mota"]');
                if (moTaInput && moTaEditor) {
                    moTaInput.value = moTaEditor.innerHTML;
                }

                // Cập nhật thông số kỹ thuật
                updateSpecifications();

                // Submit form nếu mọi thứ hợp lệ
                form.submit();
            }
        });
    }
});

// Validate form
function validateProductForm() {
    clearAllErrors();
    let isValid = true;

    // Validate tên sản phẩm
    const tensanpham = document.querySelector('[name="Tensanpham"]');
    if (!validateProductName(tensanpham.value)) {
        showError(tensanpham, `Tên sản phẩm phải từ ${MIN_NAME_LENGTH} đến ${MAX_NAME_LENGTH} ký tự`);
        isValid = false;
    }

    // Validate giá
    const gia = document.querySelector('[name="Gia"]');
    if (!validatePrice(gia.value)) {
        showError(gia, `Giá phải từ ${formatCurrency(MIN_PRICE)} đến ${formatCurrency(MAX_PRICE)}`);
        isValid = false;
    }

    // Validate số lượng
    const soluong = document.querySelector('[name="Soluongton"]');
    if (!validateQuantity(soluong.value)) {
        showError(soluong, `Số lượng phải là số nguyên dương và không vượt quá ${MAX_QUANTITY}`);
        isValid = false;
    }

    // Validate loại sản phẩm
    const loaisp = document.querySelector('[name="Loaisanpham"]');
    if (!validateProductType(loaisp.value)) {
        showError(loaisp, 'Vui lòng chọn loại sản phẩm');
        isValid = false;
    }

    // Validate thương hiệu
    const thuonghieu = document.querySelector('[name="Thuonghieu"]');
    if (!validateBrand(thuonghieu.value)) {
        showError(thuonghieu, `Thương hiệu phải từ ${MIN_BRAND_LENGTH} đến ${MAX_BRAND_LENGTH} ký tự`);
        isValid = false;
    }

    // Validate mô tả
    const mota = document.getElementById('moTaEditor');
    if (!validateDescription(mota.innerHTML)) {
        showError(mota, 'Vui lòng nhập mô tả sản phẩm');
        isValid = false;
    }

    // Validate thông số kỹ thuật
    if (!validateSpecifications()) {
        isValid = false;
    }

    // Nếu có lỗi validation, scroll đến phần tử lỗi đầu tiên
    if (!isValid) {
        const firstError = document.querySelector('.text-red-500');
        if (firstError) {
            firstError.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }

    return isValid;
}

// Cập nhật thông số kỹ thuật
function updateSpecifications() {
    const specs = {};
    const rows = document.querySelectorAll('#specTable tbody tr');
    rows.forEach(row => {
        const keyInput = row.querySelector('input[name$="].key"]');
        const valueInput = row.querySelector('input[name$="].value"]') || 
                           row.querySelector('select[name$="].value"]');
        if (keyInput && valueInput && keyInput.value.trim() && valueInput.value.trim()) {
            specs[keyInput.value.trim()] = valueInput.value.trim();
        }
    });
    document.querySelector('input[name="thongsokythuat"]').value = JSON.stringify(specs);
}

// Validation functions
function validateProductName(name) {
    return name && name.length >= MIN_NAME_LENGTH && name.length <= MAX_NAME_LENGTH;
}

function validatePrice(price) {
    const numPrice = Number(price);
    return !isNaN(numPrice) && numPrice >= MIN_PRICE && numPrice <= MAX_PRICE;
}

function validateQuantity(quantity) {
    const numQuantity = Number(quantity);
    return Number.isInteger(numQuantity) && numQuantity >= 0 && numQuantity <= MAX_QUANTITY;
}

function validateProductType(type) {
    return type && type.length > 0;
}

function validateBrand(brand) {
    return brand && brand.length >= MIN_BRAND_LENGTH && brand.length <= MAX_BRAND_LENGTH;
}

function validateDescription(description) {
    const plainText = description.replace(/<[^>]*>/g, '').trim();
    return plainText.length > 0;
}

function validateSpecifications() {
    const tbody = document.querySelector('#specTable tbody');
    const rows = tbody.querySelectorAll('tr');
    let isValid = true;

    rows.forEach((row, index) => {
        const keyInput = row.querySelector('input[name$="].key"]');
        const valueInput = row.querySelector('input[name$="].value"]') || 
                           row.querySelector('select[name$="].value"]');

        if (!keyInput.value.trim()) {
            showError(keyInput, 'Vui lòng nhập tên thông số');
            isValid = false;
        }

        if (!valueInput.value.trim()) {
            showError(valueInput, 'Vui lòng nhập giá trị');
            isValid = false;
        }
    });

    return isValid;
}

// Helper functions
function showError(element, message) {
    const errorSpan = document.createElement('span');
    errorSpan.className = 'text-red-500 text-sm mt-1';
    errorSpan.textContent = message;
    
    // Xóa lỗi cũ nếu có
    const existingError = element.parentElement.querySelector('.text-red-500');
    if (existingError) {
        existingError.remove();
    }
    
    element.classList.add('border-red-500');
    element.parentElement.appendChild(errorSpan);
}

function clearAllErrors() {
    // Xóa tất cả thông báo lỗi
    document.querySelectorAll('.text-red-500').forEach(el => {
        if (el.tagName.toLowerCase() === 'span') {
            el.textContent = '';
        }
    });
    // Xóa tất cả border đỏ
    document.querySelectorAll('.border-red-500').forEach(el => {
        el.classList.remove('border-red-500');
    });
}

function formatCurrency(number) {
    return new Intl.NumberFormat('vi-VN', { 
        style: 'currency', 
        currency: 'VND' 
    }).format(number);
}

// Real-time validation
function setupRealTimeValidation() {
    // Validate tên sản phẩm khi blur
    const tensanpham = document.querySelector('[name="Tensanpham"]');
    tensanpham?.addEventListener('blur', () => {
        if (!validateProductName(tensanpham.value)) {
            showError(tensanpham, `Tên sản phẩm phải từ ${MIN_NAME_LENGTH} đến ${MAX_NAME_LENGTH} ký tự`);
        }
    });

    // Validate giá khi blur
    const gia = document.querySelector('[name="Gia"]');
    gia?.addEventListener('blur', () => {
        if (!validatePrice(gia.value)) {
            showError(gia, `Giá phải từ ${formatCurrency(MIN_PRICE)} đến ${formatCurrency(MAX_PRICE)}`);
        }
    });

    // Validate số lượng khi blur
    const soluong = document.querySelector('[name="Soluongton"]');
    soluong?.addEventListener('blur', () => {
        if (!validateQuantity(soluong.value)) {
            showError(soluong, `Số lượng phải là số nguyên dương và không vượt quá ${MAX_QUANTITY}`);
        }
    });

    // Validate thương hiệu khi blur
    const thuonghieu = document.querySelector('[name="Thuonghieu"]');
    thuonghieu?.addEventListener('blur', () => {
        if (!validateBrand(thuonghieu.value)) {
            showError(thuonghieu, `Thương hiệu phải từ ${MIN_BRAND_LENGTH} đến ${MAX_BRAND_LENGTH} ký tự`);
        }
    });

    // Clear error khi focus vào input
    document.querySelectorAll('input, select, textarea').forEach(element => {
        element.addEventListener('focus', () => {
            const errorSpan = element.nextElementSibling;
            if (errorSpan && errorSpan.classList.contains('text-red-500')) {
                errorSpan.textContent = '';
            }
            element.classList.remove('border-red-500');
        });
    });
}
