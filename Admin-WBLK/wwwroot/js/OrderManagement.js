// Validate form
function validateOrderForm(e) {
    e.preventDefault();
    
    // Lấy các giá trị
    const ngaydat = document.getElementById('Ngaydat')?.value || '';
    const trangthai = document.getElementById('Trangthai')?.value || '';
    const tongtien = document.getElementById('Tongtien')?.value || '';
    const diachigiaohang = document.getElementById('Diachigiaohang')?.value?.trim() || '';
    const sodienthoaigh = document.getElementById('Sodienthoaigh')?.value?.trim() || '';

    let isValid = true;
    let errorMessages = [];

    // Validate ngày đặt
    if (document.getElementById('Ngaydat')) {
        const orderDate = new Date(ngaydat);
        const today = new Date();
        const ngaydatError = document.getElementById('NgaydatError');

        if (!ngaydat) {
            isValid = false;
            if (ngaydatError) {
                ngaydatError.textContent = 'Vui lòng chọn ngày đặt';
            }
            document.getElementById('Ngaydat').classList.add('border-red-500');
        }
        else if (orderDate > today) {
            isValid = false;
            if (ngaydatError) {
                ngaydatError.textContent = 'Ngày đặt không thể là ngày trong tương lai';
            }
            document.getElementById('Ngaydat').classList.add('border-red-500');
        }
    }

    // Validate trạng thái
    if (!trangthai) {
        isValid = false;
        errorMessages.push('Vui lòng chọn trạng thái đơn hàng');
        document.getElementById('Trangthai').classList.add('border-red-500');
    }

    // Validate tổng tiền
    if (!tongtien || parseFloat(tongtien) <= 0) {
        isValid = false;
        errorMessages.push('Tổng tiền phải lớn hơn 0');
        document.getElementById('Tongtien').classList.add('border-red-500');
    }

    // Validate địa chỉ giao hàng
    if (diachigiaohang) {
        if (diachigiaohang.length < 5 || diachigiaohang.length > 200) {
            isValid = false;
            errorMessages.push('Địa chỉ giao hàng phải từ 5 đến 200 ký tự');
            document.getElementById('Diachigiaohang').classList.add('border-red-500');
        }
    }

    // Validate số điện thoại giao hàng
    if (sodienthoaigh) {
        if (!/^0[0-9]{9}$/.test(sodienthoaigh)) {
            isValid = false;
            errorMessages.push('Số điện thoại không hợp lệ (phải bắt đầu bằng 0 và có 10 số)');
            document.getElementById('Sodienthoaigh').classList.add('border-red-500');
        }
    }

    // Validate chi tiết đơn hàng
    const quantities = document.querySelectorAll('[id^="Soluong_"]');
    const prices = document.querySelectorAll('[id^="Gia_"]');

    quantities.forEach((qty, index) => {
        const quantity = parseInt(qty.value);
        if (isNaN(quantity) || quantity <= 0) {
            isValid = false;
            errorMessages.push(`Số lượng sản phẩm ${index + 1} phải lớn hơn 0`);
            qty.classList.add('border-red-500');
        }
    });

    prices.forEach((price, index) => {
        const priceValue = parseFloat(price.value);
        if (isNaN(priceValue) || priceValue <= 0) {
            isValid = false;
            errorMessages.push(`Giá sản phẩm ${index + 1} phải lớn hơn 0`);
            price.classList.add('border-red-500');
        }
    });

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

// Reset validation khi input thay đổi
function resetValidation() {
    document.querySelectorAll('input, select').forEach(input => {
        input.addEventListener('input', function() {
            this.classList.remove('border-red-500');
            if (this.id === 'Ngaydat') {
                document.getElementById('NgaydatError').textContent = '';
            }
            document.getElementById('validation-summary').innerHTML = '';
        });
    });
}

// Initialize validation
document.addEventListener('DOMContentLoaded', function() {
    const form = document.querySelector('form');
    if (form) {
        // Chỉ áp dụng validation cho form Create
        if (!form.id || form.id !== 'editOrderForm') {
            form.addEventListener('submit', validateOrderForm);
            resetValidation();
        }
    }
});

// Kiểm tra khách hàng
document.getElementById('IdKh').addEventListener('input', async function() {
    const customerId = this.value;
    if (!customerId) {
        document.getElementById('tenKhachHang').value = '';
        return;
    }

    try {
        const response = await fetch(`/OrderManagement/GetCustomerInfo?id=${encodeURIComponent(customerId)}`);
        const data = await response.json();
        if (data) {
            document.getElementById('tenKhachHang').value = data.hoten;
            document.getElementById('customerError').classList.add('hidden');
        } else {
            document.getElementById('tenKhachHang').value = '';
            document.getElementById('customerError').textContent = "Không tìm thấy khách hàng";
            document.getElementById('customerError').classList.remove('hidden');
        }
    } catch (error) {
        document.getElementById('customerError').textContent = "Lỗi khi kiểm tra thông tin khách hàng";
        document.getElementById('customerError').classList.remove('hidden');
    }
});

// Biến lưu giá trị giảm giá hiện tại
let currentDiscount = 0;

// Kiểm tra mã giảm giá
document.getElementById('IdMgg')?.addEventListener('input', async function() {
    const discountId = this.value;
    const isEditMode = document.getElementById('editOrderForm') !== null; // Kiểm tra có phải trang Edit không

    if (!discountId) {
        document.getElementById('discountInfo').textContent = '';
        document.getElementById('discountError').classList.add('hidden');
        document.getElementById('discountSuccess').classList.add('hidden');
        currentDiscount = 0;
        updateTotal();
        return;
    }

    try {
        // Thêm tham số isEdit vào URL
        const response = await fetch(`/OrderManagement/GetDiscountInfo?id=${encodeURIComponent(discountId)}&isEdit=${isEditMode}`);
        const data = await response.json();
        
        if (data.success) {
            currentDiscount = data.tilechietkhau;
            document.getElementById('discountSuccess').textContent = data.message;
            document.getElementById('discountSuccess').classList.remove('hidden');
            document.getElementById('discountError').classList.add('hidden');
            document.getElementById('discountInfo').classList.remove('hidden');
        } else {
            document.getElementById('discountError').textContent = data.message;
            document.getElementById('discountError').classList.remove('hidden');
            document.getElementById('discountSuccess').classList.add('hidden');
            document.getElementById('discountInfo').classList.add('hidden');
            currentDiscount = 0;
        }
        updateTotal();
    } catch (error) {
        document.getElementById('discountError').textContent = "Lỗi khi kiểm tra mã giảm giá";
        document.getElementById('discountError').classList.remove('hidden');
        document.getElementById('discountSuccess').classList.add('hidden');
        document.getElementById('discountInfo').classList.add('hidden');
        currentDiscount = 0;
        updateTotal();
    }
});

// Thêm sản phẩm mới
function addProductRow() {
    const tbody = document.querySelector('#productTable tbody');
    const newRow = document.createElement('tr');
    newRow.innerHTML = `
        <td class="px-4 py-3 border-b">
            <input type="text" name="IdSp" class="product-id w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500" 
                   required pattern="^SP[0-9]{5}$" title="Mã sản phẩm phải bắt đầu bằng 'SP' và theo sau là 5 số" />
        </td>
        <td class="px-4 py-3 border-b">
            <input type="text" class="product-name w-full px-3 py-2 bg-gray-50 border border-gray-300 rounded-lg" readonly />
        </td>
        <td class="px-4 py-3 border-b">
            <input type="number" name="Soluong" class="product-quantity w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500" 
                   required min="1" value="1" />
        </td>
        <td class="px-4 py-3 border-b">
            <input type="number" name="Dongia" class="product-price w-full px-3 py-2 bg-gray-50 border border-gray-300 rounded-lg" readonly />
        </td>
        <td class="px-4 py-3 border-b text-center">
            <button type="button" onclick="removeProductRow(this)" class="text-red-600 hover:text-red-800">
                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                </svg>
            </button>
        </td>
    `;
    tbody.appendChild(newRow);

    // Thêm event listener cho mã sản phẩm mới
    const productInput = newRow.querySelector('.product-id');
    productInput.addEventListener('input', async function() {
        const productId = this.value;
        const row = this.closest('tr');
        if (!productId) {
            row.querySelector('.product-name').value = '';
            row.querySelector('.product-price').value = '';
            return;
        }

        try {
            const response = await fetch(`/OrderManagement/GetProductInfo?id=${encodeURIComponent(productId)}`);
            const data = await response.json();
            console.log(data);
            if (data && data.tensanpham) {
                row.querySelector('.product-name').value = data.tensanpham;
                row.querySelector('.product-price').value = data.gia;
                updateTotal();
            } else {
                row.querySelector('.product-name').value = 'Không tìm thấy sản phẩm';
                row.querySelector('.product-price').value = '';
            }
        } catch (error) {
            row.querySelector('.product-name').value = 'Lỗi khi tìm sản phẩm';
            row.querySelector('.product-price').value = '';
        }
    });

    // Thêm event listener cho số lượng
    const quantityInput = newRow.querySelector('.product-quantity');
    quantityInput.addEventListener('input', updateTotal);
}

// Xóa sản phẩm
function removeProductRow(button) {
    button.closest('tr').remove();
    updateTotal();
}

// Cập nhật tổng tiền
function updateTotal() {
    let total = 0;
    const rows = document.querySelectorAll('#productTable tbody tr');
    
    rows.forEach(row => {
        const quantity = parseFloat(row.querySelector('.product-quantity').value) || 0;
        const price = parseFloat(row.querySelector('.product-price').value) || 0;
        total += quantity * price;
    });

    // Hiển thị số tiền trước khi giảm giá
    document.getElementById('originalTotal').textContent = total.toLocaleString('vi-VN');

    // Áp dụng giảm giá nếu có
    if (currentDiscount > 0) {
        const discountAmount = total * (currentDiscount / 100);
        document.getElementById('discountAmount').textContent = 
            `Giảm: -${discountAmount.toLocaleString('vi-VN')} VNĐ`;
        document.getElementById('discountAmount').classList.remove('hidden');
        total -= discountAmount;
    } else {
        document.getElementById('discountAmount').classList.add('hidden');
    }

    document.getElementById('Tongtien').value = total;
    document.getElementById('displayTotal').textContent = total.toLocaleString('vi-VN');
}

// Initialize
document.addEventListener('DOMContentLoaded', function() {
    // Thêm một hàng sản phẩm mặc định
    addProductRow();
});

// Validate form trước khi submit
document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('createOrderForm');
    if (form) {
        form.addEventListener('submit', async function(e) {
            e.preventDefault();

            // Kiểm tra các trường bắt buộc
            const requiredFields = {
                'IdKh': 'Mã khách hàng',
                'Diachigiaohang': 'Địa chỉ giao hàng',
                'paymentMethod': 'Phương thức thanh toán'
            };

            for (const [id, label] of Object.entries(requiredFields)) {
                const field = document.getElementById(id);
                if (!field || !field.value.trim()) {
                    alert(`Vui lòng nhập ${label}`);
                    field?.focus();
                    return;
                }
            }

            // Kiểm tra bảng sản phẩm
            const productRows = document.querySelectorAll('#productTable tbody tr');
            if (productRows.length === 0) {
                alert('Vui lòng thêm ít nhất một sản phẩm');
                return;
            }

            // Thu thập thông tin chi tiết đơn hàng
            const chitietdonhangs = [];
            for (const row of productRows) {
                const idSp = row.querySelector('[name="IdSp"]').value;
                const soluong = parseInt(row.querySelector('.product-quantity').value);
                const dongia = parseFloat(row.querySelector('.product-price').value);

                if (!idSp || isNaN(soluong) || isNaN(dongia)) {
                    alert('Vui lòng kiểm tra lại thông tin sản phẩm');
                    return;
                }

                chitietdonhangs.push({ IdSp: idSp, Soluong: soluong, Dongia: dongia });
            }

            // Thêm hidden input cho chitietdonhangs
            let chitietdonhangsInput = form.querySelector('input[name="chitietdonhangs"]');
            if (!chitietdonhangsInput) {
                chitietdonhangsInput = document.createElement('input');
                chitietdonhangsInput.type = 'hidden';
                chitietdonhangsInput.name = 'chitietdonhangs';
                form.appendChild(chitietdonhangsInput);
            }
            chitietdonhangsInput.value = JSON.stringify(chitietdonhangs);

            // Log để debug
            console.log('Submitting form with chitietdonhangs:', chitietdonhangsInput.value);

            // Submit form
            form.submit();
        });
    }
});

// Xử lý hiển thị form thanh toán khi trang load
document.addEventListener('DOMContentLoaded', function() {
    // Xóa event listener cũ của paymentMethod vì giờ là readonly
    const paymentMethod = document.getElementById('Phuongthucthanhtoan')?.value;
    const onlinePaymentInfo = document.getElementById('onlinePaymentInfo');
    
    if (paymentMethod && onlinePaymentInfo) {
        if (paymentMethod === 'VNPay' || paymentMethod === 'Paypal') {
            onlinePaymentInfo.classList.remove('hidden');
        } else {
            onlinePaymentInfo.classList.add('hidden');
        }
    }
});

// Thêm các hàm xử lý sự kiện khác ở đây 

async function searchCustomer() {
    const idKh = document.getElementById('IdKh').value;
    if (!idKh) return;

    try {
        const response = await fetch(`/OrderManagement/GetCustomerInfo/${idKh}`);
        const data = await response.json();
        if (data) {
            document.getElementById('tenKhachHang').value = data.hoten;
        } else {
            alert('Không tìm thấy khách hàng');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Có lỗi xảy ra khi tìm kiếm khách hàng');
    }
}

async function checkDiscount() {
    const idMgg = document.getElementById('IdMgg').value;
    if (!idMgg) return;

    try {
        const response = await fetch(`/OrderManagement/GetDiscountInfo/${idMgg}`);
        const data = await response.json();
        if (data) {
            discountPercent = data.tilechietkhau;
            calculateFinalTotal();
        } else {
            alert('Mã giảm giá không hợp lệ');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Có lỗi xảy ra khi kiểm tra mã giảm giá');
    }
} 