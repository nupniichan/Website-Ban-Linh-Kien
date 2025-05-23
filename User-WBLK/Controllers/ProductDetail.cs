﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Website_Ban_Linh_Kien.Models;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class ProductDetail : BaseController
    {
        private readonly DatabaseContext _context;

        public ProductDetail(DatabaseContext context)
        {
            _context = context;
        }

        private ProductDetailViewModel MapSanphamToViewModel(Sanpham sp)
        {
            // Parse thongSoKyThuat từ JSON string sang Dictionary
            Dictionary<string, string> specifications = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(sp.Thongsokythuat))
            {
                try
                {
                    // Thử parse trực tiếp từ JSON object
                    var jsonDoc = JsonSerializer.Deserialize<Dictionary<string, string>>(sp.Thongsokythuat);
                    if (jsonDoc != null)
                    {
                        foreach (var kvp in jsonDoc)
                        {
                            specifications.Add(kvp.Key, kvp.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        // Nếu không được, thử parse từ array của JSON objects
                        var specList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(sp.Thongsokythuat);
                        if (specList != null)
                        {
                            foreach (var spec in specList)
                            {
                                if (spec.ContainsKey("key") && spec.ContainsKey("value"))
                                {
                                    specifications.Add(spec["key"], spec["value"]);
                                }
                            }
                        }
                    }
                    catch (Exception innerEx)
                    {
                        // Log lỗi nếu cần
                        System.Diagnostics.Debug.WriteLine($"Error parsing specifications: {innerEx.Message}");
                    }
                }
            }

            // Debug: In ra các thông số đã parse được
            foreach (var spec in specifications)
            {
                System.Diagnostics.Debug.WriteLine($"Spec: {spec.Key} = {spec.Value}");
            }

            // Gather distinct review IDs for this product as a list
            var distinctReviewIds = _context.Chitietdonhangs
                .Where(ct => ct.IdSp == sp.IdSp && ct.IdDg != null)
                .Select(ct => ct.IdDg)
                .Distinct()
                .ToList();

            // Force client evaluation on the Contains part
            var reviews = _context.Danhgia
                .AsEnumerable()  // switch to client evaluation
                .Where(dg => distinctReviewIds.Contains(dg.IdDg))
                .ToList();



            var totalReviews = reviews.Count;
            var averageRating = totalReviews > 0 ? reviews.Average(r => r.Sosao) : 0;

            // Determine if the current (authenticated) user has purchased this product
            // in an order with status "Giao thành công".
            bool hasPurchased = false;
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                hasPurchased = _context.Chitietdonhangs.Any(ct =>
                    ct.IdSp == sp.IdSp &&
                    ct.IdDhNavigation.IdKh == userId &&
                    ct.IdDhNavigation.Trangthai == "Giao thành công"
                );
            }

            // Tạo phân phối rating (1-5 sao)
            var ratingDistribution = new Dictionary<int, int>();
            for (int i = 1; i <= 5; i++)
            {
                ratingDistribution[i] = reviews.Count(r => r.Sosao == i);
            }

            // Lấy các sản phẩm liên quan dựa trên loại sản phẩm và danh mục
            var relatedProducts = _context.Sanphams
                .Where(p => p.IdSp != sp.IdSp && p.Loaisanpham == sp.Loaisanpham)
                .AsEnumerable()
                .Where(p =>
                {
                    try
                    {
                        if (string.IsNullOrEmpty(p.Thongsokythuat)) return false;
                        var currentSpecs = JsonSerializer.Deserialize<Dictionary<string, string>>(sp.Thongsokythuat);
                        var currentCategory = currentSpecs?.GetValueOrDefault("Danh mục")?.Trim().ToLower();

                        var relatedSpecs = JsonSerializer.Deserialize<Dictionary<string, string>>(p.Thongsokythuat);
                        var relatedCategory = relatedSpecs?.GetValueOrDefault("Danh mục")?.Trim().ToLower();

                        return currentCategory == relatedCategory;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error parsing specifications for product {p.IdSp}: {ex.Message}");
                        return false;
                    }
                })
                .OrderBy(p => Guid.NewGuid())  // Random order
                .Take(5)  // Lấy 5 sản phẩm thay vì 6
                .Select(p => new ProductCardViewModel
                {
                    IdSp = p.IdSp,
                    TenSp = p.Tensanpham,
                    Gia = p.Gia,
                    ImageUrl = p.Hinhanh,
                    LoaiSp = p.Loaisanpham,
                    SoLuongTon = p.Soluongton
                })
                .ToList();

            foreach (var product in relatedProducts)
            {
                System.Diagnostics.Debug.WriteLine($"Related product: {product.IdSp} - {product.TenSp}");
            }

            return new ProductDetailViewModel
            {
                Id = sp.IdSp,
                Name = sp.Tensanpham,
                Price = sp.Gia,
                ImageUrl = sp.Hinhanh,
                Description = sp.Mota,
                Specifications = specifications,
                Brand = sp.Thuonghieu,
                InStock = sp.Soluongton > 0,
                ProductCode = sp.IdSp,
                Category = sp.Loaisanpham,
                LoaiSp = sp.Loaisanpham, // Ensure this is set!
                TotalReviews = totalReviews,
                AverageRating = Math.Round(averageRating, 1),
                RatingDistribution = ratingDistribution,
                Reviews = reviews.Select(r => new ProductReviewViewModel
                {
                    UserName = r.IdKhNavigation?.Hoten ?? "Anonymous",
                    Rating = r.Sosao,
                    Comment = r.Noidung,
                    Date = r.Ngaydanhgia
                }).ToList(),
                ViewCount = sp.Soluotxem,
                PurchaseCount = sp.Damuahang,
                Rating = averageRating,
                Warranty = 24,
                SoLuongTon = sp.Soluongton,
                RelatedProducts = relatedProducts,
                HasPurchased = hasPurchased
            };
        }

        // PC Routes
        [Route("pc/{id}")]
        public ActionResult PC(string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Chitietdonhangs)
                    .ThenInclude(ct => ct.IdDgNavigation)
                        .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id && s.Loaisanpham.ToLower() == "pc");
            
            if (product == null)
                return NotFound();

            // Tăng số lượt xem và lưu ngay lập tức
            product.Soluotxem += 1;
            _context.SaveChanges();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("PC", "/productslist/pc"),
                ("Chi tiết PC", null)
            );
            return View("Index", viewModel);
        }

        // Laptop Routes
        [Route("laptop/{id}")]
        public ActionResult Laptop(string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Chitietdonhangs)
                    .ThenInclude(ct => ct.IdDgNavigation)
                        .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id && s.Loaisanpham.ToLower() == "laptop");
            
            if (product == null)
                return NotFound();

            product.Soluotxem += 1;
            _context.SaveChanges();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Laptop", "/productslist/laptop"),
                ("Chi tiết Laptop", null)
            );
            return View("Index", viewModel);
        }

        // Components Routes - Add new overload
        [Route("components/{id}")]
        public ActionResult Components(string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Chitietdonhangs)
                    .ThenInclude(ct => ct.IdDgNavigation)
                        .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id);

            if (product == null)
                return NotFound();

            product.Soluotxem += 1;
            _context.SaveChanges();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Components", "/productslist/components"),
                ("Chi tiết sản phẩm", null)
            );

            return View("Index", viewModel);
        }

        // Existing Components route
        [Route("components/{category}/{id}")]
        public ActionResult Components(string category, string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Chitietdonhangs)
                    .ThenInclude(ct => ct.IdDgNavigation)
                        .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id && s.Loaisanpham.ToLower() == category.ToLower());

            if (product == null)
                return NotFound();

            product.Soluotxem += 1;
            _context.SaveChanges();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Components", "/productslist/components"),
                (category?.ToUpper() ?? "Components", $"/productslist/components/{category}"),
                ("Chi tiết sản phẩm", null)
            );

            return View("Index", viewModel);
        }

        // Storage Routes
        [Route("storage/{id}")]
        public ActionResult Storage(string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Chitietdonhangs)
                    .ThenInclude(ct => ct.IdDgNavigation)
                        .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id && s.Loaisanpham.ToLower() == "storage");
            
            if (product == null)
                return NotFound();

            product.Soluotxem += 1;
            _context.SaveChanges();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Storage", "/productslist/storage"),
                ("Chi tiết Storage", null)
            );
            return View("Index", viewModel);
        }

        // Monitor Routes
        [Route("monitor/{id}")]
        public ActionResult Monitor(string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Chitietdonhangs)
                    .ThenInclude(ct => ct.IdDgNavigation)
                        .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id && s.Loaisanpham.ToLower() == "monitor");
            
            if (product == null)
                return NotFound();

            product.Soluotxem += 1;
            _context.SaveChanges();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Monitor", "/productslist/monitor"),
                ("Chi tiết Monitor", null)
            );
            return View("Index", viewModel);
        }

        // Audio Routes
        [Route("audio/{category}/{id}")]
        public ActionResult Audio(string category, string id)
        {
            var product = _context.Sanphams.FirstOrDefault(s => s.IdSp == id && s.Loaisanpham.ToLower() == category.ToLower());
            if (product == null)
                return NotFound();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Audio", "/productslist/audio"),
                (category?.ToUpper() ?? "Audio", $"/productslist/audio/{category}"),
                ("Chi tiết sản phẩm", null)
            );
            return View("Index", viewModel);
        }

        // Peripherals Routes
        [Route("peripherals/{category}/{id}")]
        public ActionResult Peripherals(string category, string id)
        {
            var product = _context.Sanphams.FirstOrDefault(s => s.IdSp == id && s.Loaisanpham.ToLower() == category.ToLower());
            if (product == null)
                return NotFound();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Peripherals", "/productslist/peripherals"),
                (category?.ToUpper() ?? "Peripherals", $"/productslist/peripherals/{category}"),
                ("Chi tiết sản phẩm", null)
            );
            return View("Index", viewModel);
        }

        // Network Routes
        [Route("network/{id}")]
        public ActionResult Network(string id)
        {
            var product = _context.Sanphams
                .Include(s => s.Chitietdonhangs)
                    .ThenInclude(ct => ct.IdDgNavigation)
                        .ThenInclude(d => d.IdKhNavigation)
                .FirstOrDefault(s => s.IdSp == id && s.Loaisanpham.ToLower() == "network");
            if (product == null)
                return NotFound();

            var viewModel = MapSanphamToViewModel(product);
            SetBreadcrumb(
                ("Network", "/productslist/network"),
                ("Chi tiết Network", null)
            );
            return View("Index", viewModel);
        }

    }
}
