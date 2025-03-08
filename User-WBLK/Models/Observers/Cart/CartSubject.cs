using System.Collections.Generic;
using System.Threading.Tasks;

namespace Website_Ban_Linh_Kien.Models.Observers.Cart
{
    public class CartSubject
    {
        private readonly List<ICartObserver> _observers = new List<ICartObserver>();
        private Giohang _cart;

        public void Attach(ICartObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(ICartObserver observer)
        {
            _observers.Remove(observer);
        }

        public async Task Notify()
        {
            foreach (var observer in _observers)
            {
                await observer.Update(_cart);
            }
        }

        public void SetCart(Giohang cart)
        {
            _cart = cart;
        }
    }
} 