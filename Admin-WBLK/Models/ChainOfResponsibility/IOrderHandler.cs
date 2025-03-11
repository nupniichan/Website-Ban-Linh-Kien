using System.Threading.Tasks;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.ChainOfResponsibility
{
    // Interface xử lý đơn hàng
    public interface IOrderHandler
    {
        Task<bool> Handle(Donhang order);
        IOrderHandler SetNext(IOrderHandler handler);
    }
    
    // Lớp cơ sở xử lý đơn hàng
    public abstract class OrderHandler : IOrderHandler
    {
        protected IOrderHandler _nextHandler;
        
        public IOrderHandler SetNext(IOrderHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }
        
        public abstract Task<bool> Handle(Donhang order);
    }
} 