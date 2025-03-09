using System.Threading.Tasks;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Observers
{
    public interface IDiscountSubject
    {
        void Attach(IDiscountObserver observer);
        void Detach(IDiscountObserver observer);
        Task NotifyObservers(Magiamgia discount, string action);
    }
} 