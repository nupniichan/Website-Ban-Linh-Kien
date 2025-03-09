using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Admin_WBLK.Models.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Admin_WBLK.Models.Commands
{
    public class UpdateProductCommand : IProductCommand
    {
        private readonly DatabaseContext _context;
        private readonly Sanpham _product;
        private readonly string _id;
        private readonly IFormFile _imageFile;
        private readonly IProductFactory _productFactory;
        private readonly Controller _controller;
        private readonly ITempDataDictionary _tempData;
        private readonly string _thongSoKyThuat;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        public UpdateProductCommand(
            DatabaseContext context,
            Sanpham product,
            string id,
            IFormFile imageFile,
            string thongSoKyThuat,
            IProductFactory productFactory,
            Controller controller,
            ITempDataDictionary tempData)
        {
            _context = context;
            _product = product;
            _id = id;
            _imageFile = imageFile;
            _thongSoKyThuat = thongSoKyThuat;
            _productFactory = productFactory;
            _controller = controller;
            _tempData = tempData;
        }

        public async Task<IActionResult> Execute()
        {
            try
            {
                if (_id != _product.IdSp)
                    return _controller.NotFound();

                if (!_controller.ModelState.IsValid)
                    return _controller.View(_product);

                var existingProduct = await _context.Sanphams.AsNoTracking()
                                                           .FirstOrDefaultAsync(s => s.IdSp == _id);
                if (existingProduct == null)
                    return _controller.NotFound();

                // Xử lý hình ảnh
                if (_imageFile != null && _imageFile.Length > 0)
                {
                    _product.Hinhanh = await _productFactory.ProcessImage(_imageFile);
                }
                else
                {
                    _product.Hinhanh = existingProduct.Hinhanh;
                }

                // Xử lý thông số kỹ thuật
                var specs = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(_thongSoKyThuat))
                {
                    try
                    {
                        specs = JsonSerializer.Deserialize<Dictionary<string, string>>(_thongSoKyThuat);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi khi deserialize thông số kỹ thuật: " + ex.Message);
                    }
                }

                _product.Thongsokythuat = specs.Any() 
                    ? JsonSerializer.Serialize(specs, _jsonOptions)
                    : existingProduct?.Thongsokythuat;

                // Giữ nguyên các giá trị không thay đổi
                _product.Soluotxem = existingProduct.Soluotxem;
                _product.Damuahang = existingProduct.Damuahang;

                _context.Update(_product);
                await _context.SaveChangesAsync();

                _tempData["Success"] = "Cập nhật sản phẩm thành công!";
                return _controller.RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _controller.ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                return _controller.View(_product);
            }
        }
    }
} 