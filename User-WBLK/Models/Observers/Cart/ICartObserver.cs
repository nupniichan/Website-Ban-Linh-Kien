using System.Threading.Tasks;

namespace Website_Ban_Linh_Kien.Models.Observers.Cart
{
    public interface ICartObserver
    {
        Task Update(Giohang cart);
    }
} 