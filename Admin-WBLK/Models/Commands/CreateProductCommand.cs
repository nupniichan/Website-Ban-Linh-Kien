using System;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Admin_WBLK.Models.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Admin_WBLK.Models.Commands
{
    public class CreateProductCommand : IProductCommand
    {
        private readonly DatabaseContext _context;
        private readonly Sanpham _product;
        private readonly IFormFile _imageFile;
        private readonly IProductFactory _productFactory;
        private readonly Controller _controller;
        private readonly ITempDataDictionary _tempData;
        private readonly string _thongSoKyThuat;

        public CreateProductCommand(
            DatabaseContext context,
            Sanpham product,
            IFormFile imageFile,
            string thongSoKyThuat,
            IProductFactory productFactory,
            Controller controller,
            ITempDataDictionary tempData)
        {
            _context = context;
            _product = product;
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
                if (_imageFile == null || _imageFile.Length == 0)
                {
                    _controller.ModelState.AddModelError("ImageFile", "Vui lòng chọn hình ảnh sản phẩm");
                    return _controller.View(_product);
                }

                if (string.IsNullOrEmpty(_thongSoKyThuat))
                {
                    _controller.ModelState.AddModelError("Thongsokythuat", "Thông số kỹ thuật không được để trống");
                    return _controller.View(_product);
                }

                // Sử dụng Factory để tạo sản phẩm mới
                var newProduct = await _productFactory.CreateProduct(
                    _product.Tensanpham,
                    _product.Gia,
                    _product.Soluongton,
                    _product.Thuonghieu,
                    _product.Mota,
                    _thongSoKyThuat,
                    _product.Loaisanpham,
                    _imageFile
                );

                _context.Sanphams.Add(newProduct);
                await _context.SaveChangesAsync();

                _tempData["Success"] = "Thêm sản phẩm thành công!";
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