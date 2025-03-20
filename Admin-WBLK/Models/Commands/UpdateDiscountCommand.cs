using System;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Commands
{
    public class UpdateDiscountCommand : ICommand
    {
        private readonly DatabaseContext _context;
        private readonly Magiamgia _discount;
        private readonly string _id;
        private readonly Controller _controller;
        private readonly ITempDataDictionary _tempData;

        public UpdateDiscountCommand(
            DatabaseContext context, 
            Magiamgia discount, 
            string id,
            Controller controller,
            ITempDataDictionary tempData)
        {
            _context = context;
            _discount = discount;
            _id = id;
            _controller = controller;
            _tempData = tempData;
        }

        public async Task<IActionResult> Execute()
        {
            // Kiểm tra ID
            if (_id != _discount.IdMgg)
            {
                return _controller.NotFound();
            }

            // Kiểm tra ngày hết hạn
            if (_discount.Ngayhethan <= _discount.Ngaysudung)
            {
                _controller.ModelState.AddModelError("Ngayhethan", "Ngày hết hạn phải sau ngày bắt đầu sử dụng.");
                return _controller.View(_discount);
            }

            if (!_controller.ModelState.IsValid)
            {
                return _controller.View(_discount);
            }

            try
            {
                _context.Update(_discount);
                await _context.SaveChangesAsync();
                _tempData["Success"] = "Cập nhật mã giảm giá thành công!";
                return _controller.RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscountExists(_discount.IdMgg))
                {
                    return _controller.NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool DiscountExists(string id)
        {
            return _context.Magiamgia.Any(e => e.IdMgg == id);
        }
    }
} 