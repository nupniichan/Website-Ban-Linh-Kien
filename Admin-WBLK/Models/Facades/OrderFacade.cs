using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Admin_WBLK.Models.Strategis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Admin_WBLK.Models.Facades
{
    public class OrderFacade
    {
        private readonly DatabaseContext _context;
        private readonly IOrderFilterStrategy _filterStrategy;

        public OrderFacade(
            DatabaseContext context,
            IOrderFilterStrategy filterStrategy)
        {
            _context = context;
            _filterStrategy = filterStrategy;
        }

        public async Task<(IEnumerable<Donhang> Items, int TotalItems, List<string> TrangThais)> GetOrders(
            string searchString, 
            string trangThai, 
            DateTime? tuNgay, 
            DateTime? denNgay, 
            int? pageNumber,
            int pageSize)
        {
            try
            {
                var template = new OrderListTemplate(
                    _context, 
                    _filterStrategy, 
                    pageNumber ?? 1, 
                    pageSize, 
                    searchString, 
                    trangThai, 
                    tuNgay, 
                    denNgay);
                
                var result = await template.GetData();
                
                // Danh sách trạng thái để làm dropdown filter
                var trangThais = new List<string>
                {
                    "Chờ xác nhận",
                    "Đã thanh toán",
                    "Thanh toán không thành công",
                    "Đã duyệt đơn",
                    "Đang giao",
                    "Giao thành công",
                    "Không nhận hàng",
                    "Hủy đơn",
                    "Đã kết thúc"
                };
                
                return (result.Items, result.TotalItems, trangThais);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrders - Exception: {ex.Message}");
                return (new List<Donhang>(), 0, new List<string>());
            }
        }

        public async Task<Donhang> GetOrderDetails(string id)
        {
            try
            {
                var template = new OrderDetailTemplate(_context, _filterStrategy, id);
                var result = await template.GetData();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrderDetails - Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<object>> SearchSuggestions(string term)
        {
            try
            {
                if (string.IsNullOrEmpty(term))
                    return new List<object>();

                term = term.ToLower();

                // Tìm kiếm theo mã đơn hàng
                var orderResults = await _context.Donhangs
                    .Where(d => d.IdDh.ToLower().Contains(term))
                    .Take(5)
                    .Select(d => new { id = d.IdDh, text = $"Đơn hàng: {d.IdDh}" })
                    .ToListAsync();

                // Tìm kiếm theo tên khách hàng
                var customerResults = await _context.Khachhangs
                    .Where(k => k.Hoten.ToLower().Contains(term))
                    .Take(5)
                    .Select(k => new { id = k.IdKh, text = $"Khách hàng: {k.Hoten}" })
                    .ToListAsync();

                // Kết hợp kết quả
                var results = orderResults.Cast<object>().Concat(customerResults.Cast<object>());
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SearchSuggestions - Exception: {ex.Message}");
                return new List<object>();
            }
        }

        public async Task<bool> UpdateOrderStatus(string id, string newStatus)
        {
            try
            {
                var order = await _context.Donhangs.FindAsync(id);
                if (order == null)
                    return false;

                order.Trangthai = newStatus;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateOrderStatus - Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteOrder(string id)
        {
            try
            {
                var order = await _context.Donhangs
                    .Include(d => d.Chitietdonhangs)
                    .Include(d => d.Thanhtoans)
                    .FirstOrDefaultAsync(d => d.IdDh == id);

                if (order == null)
                    return false;

                // Xóa chi tiết đơn hàng
                _context.Chitietdonhangs.RemoveRange(order.Chitietdonhangs);
                
                // Xóa thanh toán
                _context.Thanhtoans.RemoveRange(order.Thanhtoans);
                
                // Xóa đơn hàng
                _context.Donhangs.Remove(order);
                
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteOrder - Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GenerateOrderId()
        {
            string prefix = "DH";
            string date = DateTime.Now.ToString("yyyyMMdd");
            string newId;

            int counter = 1;
            do
            {
                newId = $"{prefix}{date}{counter:D4}";
                counter++;
            } while (await _context.Donhangs.AnyAsync(d => d.IdDh == newId));

            return newId;
        }

        public async Task<string> GenerateOrderDetailId()
        {
            string prefix = "CTDH";
            string date = DateTime.Now.ToString("yyyyMMdd");
            string newId;

            int counter = 1;
            do
            {
                newId = $"{prefix}{date}{counter:D4}";
                counter++;
            } while (await _context.Chitietdonhangs.AnyAsync(c => c.Idchitietdonhang == newId));

            return newId;
        }

        public async Task<string> GeneratePaymentId()
        {
            string prefix = "TT";
            string date = DateTime.Now.ToString("yyyyMMdd");
            string newId;

            int counter = 1;
            do
            {
                newId = $"{prefix}{date}{counter:D4}";
                counter++;
            } while (await _context.Thanhtoans.AnyAsync(t => t.IdTt == newId));

            return newId;
        }
    }
} 