using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Ban_Linh_Kien.Models;
using Website_Ban_Linh_Kien.Models.Factories;

namespace Website_Ban_Linh_Kien.Models.Facades
{
    // Facade Pattern: Cung cấp một giao diện đơn giản để tương tác với hệ thống con phức tạp
    public class ProfileFacade
    {
        private readonly DatabaseContext _context;

        public ProfileFacade(DatabaseContext context)
        {
            _context = context;
        }

        // Lấy thông tin tài khoản từ username
        public async Task<Taikhoan> GetAccountByUsername(string username)
        {
            return await _context.Taikhoans.FirstOrDefaultAsync(t => t.Tentaikhoan == username);
        }

        // Lấy thông tin khách hàng từ ID tài khoản
        public async Task<Khachhang> GetCustomerByAccountId(string accountId)
        {
            return await _context.Khachhangs.FirstOrDefaultAsync(k => k.IdTk == accountId);
        }

        // Lấy thông tin đầy đủ của khách hàng (bao gồm thông tin tài khoản và xếp hạng)
        public async Task<Khachhang> GetCustomerFullInfo(string accountId)
        {
            return await _context.Khachhangs
                .Include(k => k.IdTkNavigation)
                .Include(k => k.IdXephangvipNavigation)
                .FirstOrDefaultAsync(k => k.IdTk == accountId);
        }

        // Lấy danh sách đơn hàng của khách hàng
        public async Task<List<Donhang>> GetCustomerOrders(string customerId, int pageNumber, int pageSize)
        {
            return await _context.Donhangs
                .Where(o => o.IdKh == customerId)
                .OrderByDescending(o => o.Ngaydathang)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Đếm tổng số đơn hàng của khách hàng
        public async Task<int> CountCustomerOrders(string customerId)
        {
            return await _context.Donhangs
                .Where(o => o.IdKh == customerId)
                .CountAsync();
        }

        // Lấy thông tin chi tiết đơn hàng
        public async Task<Donhang> GetOrderDetail(string orderId, string customerId)
        {
            return await _context.Donhangs
                .Include(o => o.Chitietdonhangs)
                    .ThenInclude(ct => ct.IdSpNavigation)
                .FirstOrDefaultAsync(o => o.IdDh == orderId && o.IdKh == customerId);
        }

        // Lấy thông tin chi tiết đơn hàng theo ID chi tiết
        public async Task<Chitietdonhang> GetOrderDetailById(string orderDetailId)
        {
            return await _context.Chitietdonhangs
                .Include(ct => ct.IdDhNavigation)
                .FirstOrDefaultAsync(ct => ct.Idchitietdonhang == orderDetailId);
        }

        // Cập nhật thông tin khách hàng
        public async Task UpdateCustomerInfo(Khachhang khachhang)
        {
            _context.Khachhangs.Update(khachhang);
            await _context.SaveChangesAsync();
        }

        // Tạo đánh giá mới
        public async Task<string> CreateReview(Khachhang khachhang, int rating, string comment)
        {
            // Tạo ID đánh giá mới
            var lastReview = await _context.Danhgia.OrderByDescending(d => d.IdDg).FirstOrDefaultAsync();
            int nextNumber = 1;
            if (lastReview != null && lastReview.IdDg?.StartsWith("DG") == true)
            {
                var numericPart = lastReview.IdDg.Substring(2);
                if (int.TryParse(numericPart, out int parsed))
                {
                    nextNumber = parsed + 1;
                }
            }
            var newReviewId = "DG" + nextNumber.ToString("D6");

            // Tạo đánh giá mới
            var review = new Danhgia
            {
                IdDg = newReviewId,
                Sosao = rating,
                Noidung = comment,
                Ngaydanhgia = DateTime.Now,
                IdKh = khachhang.IdKh
            };

            _context.Danhgia.Add(review);
            await _context.SaveChangesAsync();

            return newReviewId;
        }

        // Cập nhật chi tiết đơn hàng với ID đánh giá
        public async Task UpdateOrderDetailWithReview(string orderDetailId, string reviewId)
        {
            var orderDetail = await _context.Chitietdonhangs.FindAsync(orderDetailId);
            if (orderDetail != null)
            {
                orderDetail.IdDg = reviewId;
                _context.Chitietdonhangs.Update(orderDetail);
                await _context.SaveChangesAsync();
            }
        }

        // Hủy đơn hàng
        public async Task CancelOrder(string orderId, string lydoHuy)
        {
            var order = await _context.Donhangs.FindAsync(orderId);
            if (order != null)
            {
                order.Trangthai = "Hủy đơn";
                order.LydoHuy = lydoHuy;
                _context.Donhangs.Update(order);
                await _context.SaveChangesAsync();
            }
        }
    }
} 