using System.Threading.Tasks;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.Observers
{
    public interface IDiscountObserver
    {
        Task Update(Magiamgia discount, string action);
    }
} 