﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Admin_WBLK.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;
using Admin_WBLK.Models.Strategis;
using Admin_WBLK.Models.Factories;
using Admin_WBLK.Models.Commands;
using Admin_WBLK.Models.Facades;
using Admin_WBLK.Models.AbstractFactories;
using Admin_WBLK.Models.States;

namespace Admin_WBLK.Controllers
{
    public class ProductManagementController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ProductFacade _productFacade;

        // JsonSerializer options to not escape Unicode characters
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        public ProductManagementController(DatabaseContext context)
        {
            _context = context;
            
            // Khởi tạo các thành phần
            var searchStrategy = new DefaultProductSearchStrategy();
            var filterStrategy = new DefaultProductFilterStrategy();
            var sortStrategy = new DefaultProductSortStrategy();
            var productFactory = new ProductFactory(context);
            var factoryProvider = new ProductFactoryProvider(context);
            
            // Khởi tạo Facade Pattern
            _productFacade = new ProductFacade(
                context,
                productFactory,
                searchStrategy,
                filterStrategy,
                sortStrategy,
                factoryProvider
            );
        }

        // GET: ProductManagement
        public async Task<IActionResult> Index(string searchString, string loaiSp, string thuongHieu, string sortOrder = "newest", int pageNumber = 1)
        {
            int pageSize = 10;
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentLoaiSp"] = loaiSp;
            ViewData["CurrentThuongHieu"] = thuongHieu;
            ViewData["CurrentSort"] = sortOrder;

            // Sử dụng Facade Pattern để lấy danh sách sản phẩm
            var model = await _productFacade.GetProducts(
                searchString,
                loaiSp,
                thuongHieu,
                sortOrder,
                pageNumber,
                pageSize
            );

            // Lấy danh sách loại sản phẩm và thương hiệu
            ViewBag.LoaiSps = await _productFacade.GetCategories();
            ViewBag.ThuongHieus = await _productFacade.GetBrands();

            return View(model);
        }

        // GET: ProductManagement/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            // Sử dụng Facade Pattern để lấy thông tin sản phẩm
            var sanpham = await _productFacade.GetProductById(id);
            if (sanpham == null)
                return NotFound();

            // Sử dụng State Pattern để xác định trạng thái sản phẩm
            var productContext = new ProductContext(sanpham);
            ViewBag.ProductState = productContext.GetStateName();
            ViewBag.CanUpdate = productContext.CanUpdate();
            ViewBag.CanDelete = productContext.CanDelete();
            ViewBag.CanSell = productContext.CanSell();

            // Sử dụng Prototype Pattern để tạo một bản sao của sản phẩm
            var productPrototype = new Models.Prototypes.ProductPrototype(sanpham);
            ViewBag.ClonedProduct = productPrototype.Clone();

            // Sử dụng Decorator Pattern để hiển thị thông tin sản phẩm
            var baseProduct = new Models.Decorators.ConcreteProduct(sanpham);
            
            // Kiểm tra nếu sản phẩm có giảm giá
            bool hasDiscount = sanpham.Gia > 0 && sanpham.Soluongton > 10;
            if (hasDiscount)
            {
                // Áp dụng decorator giảm giá 10% cho sản phẩm có số lượng tồn > 10
                var discountedProduct = new Models.Decorators.DiscountedProduct(baseProduct, 10);
                ViewBag.ProductName = discountedProduct.GetName();
                ViewBag.ProductPrice = discountedProduct.GetPrice();
                ViewBag.ProductDescription = discountedProduct.GetDescription();
            }
            else if (sanpham.Soluotxem > 100)
            {
                // Áp dụng decorator sản phẩm nổi bật cho sản phẩm có lượt xem > 100
                var featuredProduct = new Models.Decorators.FeaturedProduct(baseProduct);
                ViewBag.ProductName = featuredProduct.GetName();
                ViewBag.ProductPrice = featuredProduct.GetPrice();
                ViewBag.ProductDescription = featuredProduct.GetDescription();
            }
            else
            {
                // Sử dụng thông tin sản phẩm gốc
                ViewBag.ProductName = baseProduct.GetName();
                ViewBag.ProductPrice = baseProduct.GetPrice();
                ViewBag.ProductDescription = baseProduct.GetDescription();
            }

            return View(sanpham);
        }

        // GET: ProductManagement/Create
        public IActionResult Create()
        {
            // Sử dụng Singleton Pattern để lấy danh sách loại sản phẩm và thương hiệu
            var configManager = Models.Singletons.ProductConfigurationManager.Instance;
            ViewBag.Categories = configManager.GetProductCategories();
            ViewBag.Brands = configManager.GetProductBrands();
            
            return View();
        }

        // POST: ProductManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSp,Tensanpham,Gia,Soluongton,Thuonghieu,Mota,Thongsokythuat,Loaisanpham,Hinhanh,Soluotxem,Damuahang")] Sanpham sanpham, IFormFile? imageFile)
        {
            // Remove fields that are auto-set
            ModelState.Remove("IdSp");
            ModelState.Remove("Hinhanh");
            ModelState.Remove("Soluotxem");
            ModelState.Remove("Damuahang");

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                    foreach (var error in modelState.Errors)
                        Console.WriteLine(error.ErrorMessage);
                return View(sanpham);
            }

            // Sử dụng Command Pattern thông qua Facade để tạo sản phẩm
            var thongSoKyThuat = Request.Form["thongsokythuat"].ToString();
            return await _productFacade.CreateProduct(
                sanpham,
                imageFile,
                thongSoKyThuat,
                this,
                TempData
            );
        }

        // GET: ProductManagement/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            // Sử dụng Facade Pattern để lấy thông tin sản phẩm
            var sanpham = await _productFacade.GetProductById(id);
            if (sanpham == null)
                return NotFound();

            return View(sanpham);
        }

        // POST: ProductManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdSp,Tensanpham,Gia,Soluongton,Loaisanpham,Thuonghieu,Hinhanh,Mota,Thongsokythuat")] Sanpham sanpham, IFormFile? imageFile)
        {
            ModelState.Remove("IdNvNavigation");
            ModelState.Remove("IdNv");

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                    foreach (var error in modelState.Errors)
                        Console.WriteLine(error.ErrorMessage);
                return View(sanpham);
            }

            // Kiểm tra trạng thái sản phẩm trước khi cập nhật
            var currentProduct = await _productFacade.GetProductById(id);
            if (currentProduct == null)
                return NotFound();

            // Sử dụng State Pattern để kiểm tra xem sản phẩm có thể được cập nhật không
            var productContext = new ProductContext(currentProduct);
            if (!productContext.CanUpdate())
            {
                TempData["ErrorMessage"] = "Sản phẩm không thể được cập nhật trong trạng thái hiện tại.";
                return RedirectToAction(nameof(Index));
            }

            // Sử dụng Command Pattern thông qua Facade để cập nhật sản phẩm
            var thongSoKyThuat = Request.Form["thongsokythuat"].ToString();
            return await _productFacade.UpdateProduct(
                id,
                sanpham,
                imageFile,
                thongSoKyThuat,
                this,
                TempData
            );
        }

        // GET: ProductManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            // Sử dụng Facade Pattern để lấy thông tin sản phẩm
            var sanpham = await _productFacade.GetProductById(id);
            if (sanpham == null)
                return NotFound();

            // Sử dụng State Pattern để kiểm tra xem sản phẩm có thể bị xóa không
            var productContext = new ProductContext(sanpham);
            if (!productContext.CanDelete())
            {
                TempData["ErrorMessage"] = "Sản phẩm không thể bị xóa trong trạng thái hiện tại.";
                return RedirectToAction(nameof(Index));
            }

            return View(sanpham);
        }

        // POST: ProductManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Kiểm tra trạng thái sản phẩm trước khi xóa
            var sanpham = await _productFacade.GetProductById(id);
            if (sanpham == null)
                return NotFound();

            // Sử dụng State Pattern để kiểm tra xem sản phẩm có thể bị xóa không
            var productContext = new ProductContext(sanpham);
            if (!productContext.CanDelete())
            {
                TempData["ErrorMessage"] = "Sản phẩm không thể bị xóa trong trạng thái hiện tại.";
                return RedirectToAction(nameof(Index));
            }

            // Sử dụng Command Pattern thông qua Facade để xóa sản phẩm
            return await _productFacade.DeleteProduct(id, this, TempData);
        }

        private bool SanphamExists(string id)
        {
            return _context.Sanphams.Any(e => e.IdSp == id);
        }

        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term))
                return Json(new List<object>());

            term = term.ToLower();
            try
            {
                var suggestions = await _context.Sanphams
                    .Where(s => s.IdSp.ToLower().Contains(term) ||
                                s.Tensanpham.ToLower().Contains(term))
                    .Take(5)
                    .Select(s => new
                    {
                        s.IdSp,
                        s.Tensanpham,
                        MaSP = "Mã SP: " + s.IdSp
                    })
                    .ToListAsync();

                return Json(suggestions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchSuggestions: {ex.Message}");
                return Json(new List<object>());
            }
        }
    }
}
