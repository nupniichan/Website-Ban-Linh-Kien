using System;
using System.Collections.Generic;

namespace Admin_WBLK.Models.Mementos
{
    // Lớp lưu trữ trạng thái đơn hàng
    public class OrderState
    {
        public string Trangthai { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public string NguoiCapNhat { get; set; }
    }
    
    // Lớp Memento lưu trữ trạng thái
    public class OrderMemento
    {
        private readonly string _trangthai;
        private readonly DateTime _ngayCapNhat;
        
        public OrderMemento(string trangthai)
        {
            _trangthai = trangthai;
            _ngayCapNhat = DateTime.Now;
        }
        
        public string GetTrangthai()
        {
            return _trangthai;
        }
        
        public DateTime GetNgayCapNhat()
        {
            return _ngayCapNhat;
        }
    }
    
    // Lớp Originator - đối tượng cần lưu trạng thái
    public class OrderOriginator
    {
        private string _trangthai;
        
        public void SetTrangthai(string trangthai)
        {
            _trangthai = trangthai;
        }
        
        public OrderMemento SaveToMemento()
        {
            return new OrderMemento(_trangthai);
        }
        
        public void RestoreFromMemento(OrderMemento memento)
        {
            _trangthai = memento.GetTrangthai();
        }
    }
    
    // Lớp Caretaker - quản lý lịch sử trạng thái
    public class OrderCaretaker
    {
        private static Dictionary<string, List<OrderState>> _orderStateHistory = new Dictionary<string, List<OrderState>>();
        
        public void SaveState(string orderId, string status, string nguoiCapNhat)
        {
            var state = new OrderState
            {
                Trangthai = status,
                NgayCapNhat = DateTime.Now,
                NguoiCapNhat = nguoiCapNhat
            };
            
            if (!_orderStateHistory.ContainsKey(orderId))
            {
                _orderStateHistory[orderId] = new List<OrderState>();
            }
            
            _orderStateHistory[orderId].Add(state);
        }
        
        public List<OrderState> GetHistory(string orderId)
        {
            if (_orderStateHistory.ContainsKey(orderId))
            {
                return _orderStateHistory[orderId];
            }
            
            return new List<OrderState>();
        }
    }
} 