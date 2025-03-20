using System.Threading.Tasks;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Observers
{
    public interface IRevenueObserver
    {
        Task Update(Donhang order, string action);
        Task Update(string message);
    }
} 