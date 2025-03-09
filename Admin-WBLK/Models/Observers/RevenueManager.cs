using System.Collections.Generic;
using System.Threading.Tasks;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Observers
{
    public class RevenueManager : IRevenueSubject
    {
        private readonly List<IRevenueObserver> _observers = new List<IRevenueObserver>();

        public void Attach(IRevenueObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IRevenueObserver observer)
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