using System.Collections.Generic;
using System.Threading.Tasks;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Observers
{
    public class OrderManager : IOrderSubject
    {
        private readonly List<IOrderObserver> _observers = new List<IOrderObserver>();

        public void Attach(IOrderObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IOrderObserver observer)
        {
            _observers.Remove(observer);
        }

        public async Task NotifyObservers(Donhang order, string action)
        {
            foreach (var observer in _observers)
            {
                await observer.Update(order, action);
            }
        }
    }
} 