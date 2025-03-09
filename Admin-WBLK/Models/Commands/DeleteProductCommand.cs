using System;
using System.IO;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Admin_WBLK.Models.Commands
{
    public class DeleteProductCommand : IProductCommand
    {
        private readonly DatabaseContext _context;
        private readonly string _id;
        private readonly Controller _controller;
        private readonly ITempDataDictionary _tempData;

        public DeleteProductCommand(
            DatabaseContext context,
            string id,
            Controller controller,
            ITempDataDictionary tempData)
        {
            _context = context;
            _id = id;
            _controller = controller;
            _tempData = tempData;
        }

        public async Task<IActionResult> Execute()
        {
            try
            {
                var product = await _context.Sanphams.FindAsync(_id);
                if (product == null)
                {
                    return _controller.NotFound();
                }

                // Xóa hình ảnh sản phẩm nếu có
                if (!string.IsNullOrEmpty(product.Hinhanh))
                {
                    var imagePath = Path.Combine(
                        Directory.GetCurrentDirectory(), 
                        "wwwroot", 
                        product.Hinhanh.TrimStart('/').Replace("/", "\\"));
                    
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                _context.Sanphams.Remove(product);
                await _context.SaveChangesAsync();

                _tempData["Success"] = "Xóa sản phẩm thành công!";
                return _controller.RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _tempData["Error"] = $"Có lỗi xảy ra: {ex.Message}";
                return _controller.RedirectToAction("Index");
            }
        }
    }
} 