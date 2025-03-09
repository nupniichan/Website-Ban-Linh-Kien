using System;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Admin_WBLK.Models.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Admin_WBLK.Models.Commands
{
    public class CreateDiscountCommand : ICommand
    {
        private readonly DatabaseContext _context;
        private readonly Magiamgia _discount;
        private readonly IDiscountFactory _discountFactory;
        private readonly Controller _controller;
        private readonly ITempDataDictionary _tempData;

        public CreateDiscountCommand(
            DatabaseContext context, 
            Magiamgia discount, 
            IDiscountFactory discountFactory,
            Controller controller,
            ITempDataDictionary tempData)
        {
            _context = context;
            _discount = discount;
            _discountFactory = discountFactory;
            _controller = controller;
            _tempData = tempData;
        }

        public async Task<IActionResult> Execute()
        {
            // Kiểm tra ngày sử dụng
            if (_discount.Ngaysudung < DateOnly.FromDateTime(DateTime.Today))
            {
                return ValidationError("Ngaysudung", "Ngày bắt đầu sử dụng không được trong quá khứ.");
            }
            
            // Kiểm tra ngày hết hạn
            if (_discount.Ngayhethan <= _discount.Ngaysudung)
            {
                return ValidationError("Ngayhethan", "Ngày hết hạn phải sau ngày bắt đầu sử dụng.");
            }

            // Tạo ID nếu chưa có
            if (string.IsNullOrEmpty(_discount.IdMgg))
            {
                _discount.IdMgg = await _discountFactory.GenerateNextDiscountId();
            }

            // Thêm vào cơ sở dữ liệu
            _context.Add(_discount);
            await _context.SaveChangesAsync();
            
            // Thông báo thành công
            _tempData["Success"] = "Thêm mã giảm giá thành công!";
            
            // Chuyển hướng về trang danh sách
            return _controller.RedirectToAction("Index");
        }

        private IActionResult ValidationError(string key, string errorMessage)
        {
            _controller.ModelState.AddModelError(key, errorMessage);
            return _controller.View(_discount);
        }
    }
} 