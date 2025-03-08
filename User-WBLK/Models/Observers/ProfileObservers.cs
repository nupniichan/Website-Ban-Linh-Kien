using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Models.Observers
{
    // Interface cho Subject (đối tượng được quan sát)
    public interface IProfileSubject
    {
        void RegisterObserver(IProfileObserver observer);
        void RemoveObserver(IProfileObserver observer);
        Task NotifyObservers(ProfileEventType eventType, object data);
    }

    // Interface cho Observer (đối tượng quan sát)
    public interface IProfileObserver
    {
        Task Update(ProfileEventType eventType, object data);
    }

    // Enum định nghĩa các loại sự kiện
    public enum ProfileEventType
    {
        ProfileUpdated,
        ReviewSubmitted,
        OrderCancelled
    }

    // Lớp cơ sở cho Subject
    public class ProfileSubject : IProfileSubject
    {
        private readonly List<IProfileObserver> _observers = new List<IProfileObserver>();

        public void RegisterObserver(IProfileObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void RemoveObserver(IProfileObserver observer)
        {
            _observers.Remove(observer);
        }

        public async Task NotifyObservers(ProfileEventType eventType, object data)
        {
            foreach (var observer in _observers)
            {
                await observer.Update(eventType, data);
            }
        }
    }

    // Observer cụ thể: Ghi log sự kiện
    public class LoggingObserver : IProfileObserver
    {
        public async Task Update(ProfileEventType eventType, object data)
        {
            string message = $"[{DateTime.Now}] Event: {eventType}";
            
            switch (eventType)
            {
                case ProfileEventType.ProfileUpdated:
                    if (data is Khachhang khachhang)
                    {
                        message += $" - Customer ID: {khachhang.IdKh}, Name: {khachhang.Hoten}";
                    }
                    break;
                case ProfileEventType.ReviewSubmitted:
                    if (data is Danhgia review)
                    {
                        message += $" - Review ID: {review.IdDg}, Rating: {review.Sosao}";
                    }
                    break;
                case ProfileEventType.OrderCancelled:
                    if (data is Donhang order)
                    {
                        message += $" - Order ID: {order.IdDh}, Reason: {order.LydoHuy}";
                    }
                    break;
            }

            // Ghi log vào console (trong thực tế có thể ghi vào file hoặc database)
            System.Diagnostics.Debug.WriteLine(message);
            await Task.CompletedTask;
        }
    }

    // Observer cụ thể: Gửi thông báo
    public class NotificationObserver : IProfileObserver
    {
        private readonly DatabaseContext _context;

        public NotificationObserver(DatabaseContext context)
        {
            _context = context;
        }

        public async Task Update(ProfileEventType eventType, object data)
        {
            string title = string.Empty;
            string content = string.Empty;
            string recipientId = string.Empty;

            switch (eventType)
            {
                case ProfileEventType.ProfileUpdated:
                    if (data is Khachhang khachhang)
                    {
                        title = "Cập nhật thông tin tài khoản";
                        content = "Thông tin tài khoản của bạn đã được cập nhật thành công.";
                        recipientId = khachhang.IdKh;
                    }
                    break;
                case ProfileEventType.ReviewSubmitted:
                    if (data is Danhgia review)
                    {
                        title = "Đánh giá sản phẩm";
                        content = "Cảm ơn bạn đã đánh giá sản phẩm.";
                        recipientId = review.IdKh;
                    }
                    break;
                case ProfileEventType.OrderCancelled:
                    if (data is Donhang order)
                    {
                        title = "Hủy đơn hàng";
                        content = $"Đơn hàng {order.IdDh} đã được hủy với lý do: {order.LydoHuy}";
                        recipientId = order.IdKh;

                        // Thông báo cho admin
                        await SendNotificationToAdmin(order);
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(recipientId))
            {
                // Trong thực tế, có thể lưu thông báo vào database hoặc gửi email/SMS
                System.Diagnostics.Debug.WriteLine($"Notification to {recipientId}: {title} - {content}");
            }

            await Task.CompletedTask;
        }

        private async Task SendNotificationToAdmin(Donhang order)
        {
            // Trong thực tế, có thể gửi thông báo cho admin qua email hoặc hệ thống nội bộ
            string adminMessage = $"Đơn hàng {order.IdDh} đã bị hủy bởi khách hàng {order.IdKh} với lý do: {order.LydoHuy}";
            System.Diagnostics.Debug.WriteLine($"Admin notification: {adminMessage}");
            
            await Task.CompletedTask;
        }
    }

    // Observer cụ thể: Cập nhật thống kê
    public class StatisticsObserver : IProfileObserver
    {
        private readonly DatabaseContext _context;

        public StatisticsObserver(DatabaseContext context)
        {
            _context = context;
        }

        public async Task Update(ProfileEventType eventType, object data)
        {
            switch (eventType)
            {
                case ProfileEventType.ReviewSubmitted:
                    if (data is Danhgia review)
                    {
                        await UpdateProductRating(review);
                    }
                    break;
                case ProfileEventType.OrderCancelled:
                    if (data is Donhang order)
                    {
                        await UpdateCancellationStatistics(order);
                    }
                    break;
            }
        }

        private async Task UpdateProductRating(Danhgia review)
        {
            // Trong thực tế, có thể cập nhật điểm đánh giá trung bình của sản phẩm
            System.Diagnostics.Debug.WriteLine($"Updating product rating statistics for review {review.IdDg}");
            await Task.CompletedTask;
        }

        private async Task UpdateCancellationStatistics(Donhang order)
        {
            // Trong thực tế, có thể cập nhật thống kê về đơn hàng bị hủy
            System.Diagnostics.Debug.WriteLine($"Updating cancellation statistics for order {order.IdDh}");
            await Task.CompletedTask;
        }
    }
} 