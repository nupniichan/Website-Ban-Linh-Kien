using System.Collections.Generic;
using System.Threading.Tasks;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Observers
{
    public class DiscountManager : IDiscountSubject
    {
        private readonly List<IDiscountObserver> _observers = new List<IDiscountObserver>();

        public void Attach(IDiscountObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IDiscountObserver observer)
        {
            _observers.Remove(observer);
        }

        public async Task NotifyObservers(Magiamgia discount, string action)
        {
            foreach (var observer in _observers)
            {
                await observer.Update(discount, action);
            }
        }
    }
} 