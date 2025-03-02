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
    function addProductRow(product, quantity = 1) {
        // Kiểm tra xem bảng đã tồn tại chưa
        const table = document.querySelector('#productTable tbody');
        if (!table) {
            console.error('Không tìm thấy bảng sản phẩm');
            return;
        }

        // Nếu không có product được truyền vào, thêm một dòng trống
        if (!product) {
            const row = document.createElement('tr');
            const rowId = new Date().getTime(); // Tạo ID duy nhất cho dòng
            
            row.innerHTML = `
                <td>
                    <input type="text" name="IdSp" class="form-control product-id w-full px-4 py-2 border border-gray-300 rounded-lg" 
                           placeholder="Mã SP" required pattern="^SP[0-9]{5}$" 
                           title="Mã sản phẩm phải bắt đầu bằng 'SP' và theo sau là 5 số" />
                </td>
                <td>
                    <input type="text" class="form-control product-name w-full px-4 py-2 border border-gray-300 rounded-lg" 
                           placeholder="Tên sản phẩm" readonly />
                </td>
                <td>
                    <input type="number" 
                        class="form-control quantity-input w-full px-4 py-2 border border-gray-300 rounded-lg" 
                        value="1"
                        min="1" 
                        onchange="updateRowTotal(this.parentElement.parentElement)">
                </td>
                <td>
                    <input type="number" 
                        class="form-control price-input w-full px-4 py-2 border border-gray-300 rounded-lg" 
                        placeholder="Đơn giá"
                        min="0" 
                        onchange="updateRowTotal(this.parentElement.parentElement)">
                    <span class="product-price hidden" data-price="0"></span>
                </td>
                <td class="row-total px-4 py-2">0 VNĐ</td>
                <td>
                    <button type="button" 
                            class="btn btn-danger btn-sm px-3 py-1 bg-red-500 text-white rounded hover:bg-red-600"
                            onclick="removeProductRow(this.parentElement.parentElement)">
                        Xóa
                    </button>
                </td>
            `;

            // Thêm dòng vào bảng
            table.appendChild(row);
            
            // Thêm event listener cho input mã sản phẩm để tự động điền thông tin
            const productIdInput = row.querySelector('[name="IdSp"]');

            // Thêm debounce function để tránh gọi API quá nhiều lần
            function debounce(func, timeout = 300) {
                let timer;
                return (...args) => {
                    clearTimeout(timer);
                    timer = setTimeout(() => { func.apply(this, args); }, timeout);
                };
            }

            // Hàm tìm kiếm sản phẩm
            const searchProduct = async function(value) {
                // Kiểm tra xem row có còn tồn tại trong DOM không
                if (!row.isConnected) return;
                
                const productNameField = row.querySelector('.product-name');
                const priceInput = row.querySelector('.price-input');
                const priceElement = row.querySelector('.product-price');
                const quantityInput = row.querySelector('.quantity-input');
                
                if (!productNameField || !priceInput || !priceElement || !quantityInput) return;
                
                // Nếu không có giá trị, xóa tất cả thông tin sản phẩm
                if (!value) {
                    productNameField.value = '';
                    productNameField.classList.remove('text-red-500');
                    priceInput.value = '';
                    priceElement.dataset.price = '0';
                    updateRowTotal(row);
                    return;
                }
                
                // Chỉ tìm kiếm khi có ít nhất 2 ký tự
                if (value.length < 2) return;
                
                try {
                    const response = await fetch(`/OrderManagement/GetProductInfo?id=${encodeURIComponent(value)}`);
                    const data = await response.json();
                    
                    if (data && data.tensanpham) {
                        productNameField.value = data.tensanpham;
                        productNameField.classList.remove('text-red-500');
                        priceInput.value = data.gia;
                        priceElement.dataset.price = data.gia;
                        
                        // Cập nhật số lượng tối đa
                        quantityInput.max = data.soluongton;
                        
                        updateRowTotal(row);
                    } else {
                        // Hiển thị thông báo "Không tìm thấy sản phẩm" ngay khi đang gõ
                        productNameField.value = 'Không tìm thấy sản phẩm';
                        productNameField.classList.add('text-red-500');
                        priceInput.value = '';
                        priceElement.dataset.price = '0';
                        updateRowTotal(row);
                    }
                } catch (error) {
                    console.error('Error:', error);
                    if (row.isConnected && productNameField) {
                        productNameField.value = 'Lỗi khi tìm kiếm sản phẩm';
                        productNameField.classList.add('text-red-500');
                    }
                }
            };

            // Sử dụng debounce để tránh gọi API quá nhiều lần khi người dùng đang gõ
            const debouncedSearch = debounce(searchProduct, 300);

            // Thêm event listener cho input để tìm kiếm khi người dùng gõ
            productIdInput.addEventListener('input', function() {
                const value = this.value;
                const productNameField = row.querySelector('.product-name');
                
                // Nếu xóa hết ID, xóa luôn thông tin sản phẩm
                if (!value) {
                    if (productNameField) {
                        productNameField.value = '';
                        productNameField.classList.remove('text-red-500');
                    }
                    const priceInput = row.querySelector('.price-input');
                    if (priceInput) priceInput.value = '';
                    const priceElement = row.querySelector('.product-price');
                    if (priceElement) priceElement.dataset.price = '0';
                    updateRowTotal(row);
                    return;
                }
                
                // Nếu đang có thông báo lỗi, xóa đi để chuẩn bị tìm kiếm mới
                if (productNameField) {
                    if (productNameField.value === 'Không tìm thấy sản phẩm' || productNameField.value === 'Lỗi khi tìm kiếm sản phẩm') {
                        productNameField.value = '';
                    }
                    productNameField.classList.remove('text-red-500');
                }
                
                // Tìm kiếm sản phẩm khi người dùng gõ
                debouncedSearch(value);
            });

            // Vẫn giữ event blur để xử lý khi người dùng rời khỏi trường nhập
            productIdInput.addEventListener('blur', function() {
                const productId = this.value;
                searchProduct(productId);
            });
            
            return;
        }

        // Kiểm tra sản phẩm đã tồn tại
        const existingRow = document.querySelector(`tr[data-product-id="${product.idSp}"]`);
        if (existingRow) {
            // Nếu sản phẩm đã tồn tại, cập nhật số lượng
            const quantityInput = existingRow.querySelector('input[type="number"]');
            quantityInput.value = parseInt(quantityInput.value) + quantity;
            updateRowTotal(existingRow);
            return;
        }

        // Tạo dòng mới
        const row = document.createElement('tr');
        row.setAttribute('data-product-id', product.idSp);

        // Format giá tiền
        const formattedPrice = new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND'
        }).format(product.gia);

        // Nội dung HTML của dòng
        row.innerHTML = `
            <td><input type="text" name="IdSp" class="form-control product-id" placeholder="Mã SP" required /></td>
            <td>${product.tensanpham}</td>
            <td>
                <input type="number" 
                    class="form-control quantity-input" 
                    value="${quantity}"
                    min="1" 
                    max="${product.soluongton}"
                    onchange="updateRowTotal(this.parentElement.parentElement)">
            </td>
            <td class="product-price" data-price="${product.gia}">${formattedPrice}</td>
            <td class="row-total"></td>
            <td>
                <button type="button" 
                        class="btn btn-danger btn-sm"
                        onclick="removeProductRow(this.parentElement.parentElement)">
                    Xóa
                </button>
            </td>
        `;

        // Thêm dòng vào bảng
        table.appendChild(row);

        // Cập nhật tổng tiền của dòng
        updateRowTotal(row);
    }

    // Hàm cập nhật tổng tiền của một dòng
    function updateRowTotal(row) {
        const quantityInput = row.querySelector('.quantity-input');
        const quantity = parseInt(quantityInput?.value || 0);
        
        let price = 0;
        const priceElement = row.querySelector('.product-price');
        const priceInput = row.querySelector('.price-input');
        
        if (priceElement && priceElement.dataset.price) {
            price = parseFloat(priceElement.dataset.price);
        } else if (priceInput) {
            price = parseFloat(priceInput.value || 0);
            // Cập nhật data-price nếu có nhập giá mới
            if (priceInput.value) {
                const hiddenPriceElement = row.querySelector('.product-price');
                if (hiddenPriceElement) {
                    hiddenPriceElement.dataset.price = price;
                }
            }
        }
        
        const total = quantity * price;

        // Format và hiển thị tổng tiền
        const formattedTotal = new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND'
        }).format(total);

        const rowTotalElement = row.querySelector('.row-total');
        if (rowTotalElement) {
            rowTotalElement.textContent = formattedTotal;
        }

        // Cập nhật tổng tiền đơn hàng
        updateOrderTotal();
    }

    // Hàm xóa một dòng sản phẩm
    function removeProductRow(row) {
        row.remove();
        updateOrderTotal();
    }

    // Hàm cập nhật tổng tiền đơn hàng
    function updateOrderTotal() {
        let subtotal = 0;
        const rows = document.querySelectorAll('#productTable tbody tr');
        
        rows.forEach(row => {
            const quantityInput = row.querySelector('.quantity-input');
            const priceElement = row.querySelector('.product-price');
            
            if (quantityInput && priceElement && priceElement.dataset.price) {
                const quantity = parseInt(quantityInput.value || 0);
                const price = parseFloat(priceElement.dataset.price || 0);
                subtotal += quantity * price;
            }
        });

        // Lấy tỉ lệ giảm giá (nếu có)
        const discountRate = currentDiscount / 100;
        const discount = subtotal * discountRate;
        const total = subtotal - discount;

        // Cập nhật hiển thị - thêm null checks
        const subtotalElement = document.getElementById('originalTotal');
        const discountElement = document.getElementById('discountAmount');
        const totalElement = document.getElementById('displayTotal');
        const totalInputElement = document.getElementById('Tongtien');
        
        if (subtotalElement) subtotalElement.textContent = formatCurrency(subtotal);
        
        if (discountElement) {
            if (discount > 0) {
                discountElement.textContent = `Giảm giá: ${formatCurrency(discount)}`;
                discountElement.classList.remove('hidden');
            } else {
                discountElement.classList.add('hidden');
            }
        }
        
        if (totalElement) totalElement.textContent = formatCurrency(total);
        if (totalInputElement) totalInputElement.value = total;
    }

    // Hàm format tiền tệ
    function formatCurrency(amount) {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND'
        }).format(amount);
    }

    // Khởi tạo khi trang load
    document.addEventListener('DOMContentLoaded', function() {
        // Nếu đang ở trang chỉnh sửa, load sản phẩm hiện có
        const existingProducts = document.querySelectorAll('#productTable tbody tr');
        existingProducts.forEach(row => {
            updateRowTotal(row);
        });
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

                let isValid = true;
                let errorMessages = [];

                for (const [id, label] of Object.entries(requiredFields)) {
                    const field = document.getElementById(id);
                    if (!field || !field.value.trim()) {
                        errorMessages.push(`Vui lòng nhập ${label}`);
                        field?.classList.add('border-red-500');
                        isValid = false;
                    }
                }

                // Kiểm tra bảng sản phẩm
                const productRows = document.querySelectorAll('#productTable tbody tr');
                if (productRows.length === 0) {
                    errorMessages.push('Vui lòng thêm ít nhất một sản phẩm');
                    isValid = false;
                }

                // Hiển thị lỗi nếu có
                const validationSummary = document.getElementById('validation-summary');
                if (!isValid && validationSummary) {
                    validationSummary.innerHTML = errorMessages.map(msg => `<li>${msg}</li>`).join('');
                    validationSummary.classList.remove('hidden');
                    return;
                }

                // Thu thập thông tin chi tiết đơn hàng
                const chitietdonhangs = [];
                let hasProductError = false;

                for (const row of productRows) {
                    const idSp = row.querySelector('[name="IdSp"]').value;
                    const soluong = parseInt(row.querySelector('.quantity-input').value);
                    const dongia = parseFloat(row.querySelector('.product-price').dataset.price);
                    const productName = row.querySelector('.product-name')?.value;

                    if (!idSp || isNaN(soluong) || isNaN(dongia) || dongia <= 0 || 
                        productName === 'Không tìm thấy sản phẩm' || productName === 'Lỗi khi tìm kiếm sản phẩm') {
                        row.classList.add('bg-red-50');
                        hasProductError = true;
                        continue;
                    }

                    chitietdonhangs.push({ IdSp: idSp, Soluong: soluong, Dongia: dongia });
                }

                if (hasProductError) {
                    if (validationSummary) {
                        validationSummary.innerHTML = '<li>Vui lòng kiểm tra lại thông tin sản phẩm</li>';
                        validationSummary.classList.remove('hidden');
                    }
                    return;
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
            if (paymentMethod === 'Momo' || paymentMethod === 'Paypal') {
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