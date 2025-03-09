using System;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Strategis
{
    public class DeleteDiscountOperation : DiscountOperationTemplate
    {
        private readonly Controller _controller;

        public DeleteDiscountOperation(DatabaseContext context, Controller controller) : base(context)
        {
            _controller = controller;
        }

        protected override async Task<Magiamgia> FindDiscount(string id)
        {
            return await _context.Magiamgia.FindAsync(id);
        }

        protected override async Task<bool> ProcessDiscountOperation(Magiamgia discount)
        {
            _context.Magiamgia.Remove(discount);
            await _context.SaveChangesAsync();
            _controller.TempData["Success"] = "Xóa mã giảm giá thành công!";
            return true;
        }

        protected override IActionResult GetResult(bool operationResult, Magiamgia discount)
        {
            return _controller.RedirectToAction("Index");
        }

        protected override IActionResult GetNotFoundResult()
        {
            return _controller.NotFound();
        }
    }
} 