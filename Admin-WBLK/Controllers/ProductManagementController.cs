using System;
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

            return View(sanpham);
        }

        // GET: ProductManagement/Create
        public IActionResult Create()
        {
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

            return View(sanpham);
        }

        // POST: ProductManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
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
