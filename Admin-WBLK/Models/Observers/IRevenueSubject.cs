using System.Threading.Tasks;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Observers
{
    public interface IRevenueSubject
    {
        void Attach(IRevenueObserver observer);
        void Detach(IRevenueObserver observer);
        Task NotifyObservers(Donhang order, string action);
    }
} 