using System.Threading.Tasks;

namespace Website_Ban_Linh_Kien.Models.Factories.Cart
{
    public interface ICartFactory
    {
        Task<Giohang> CreateCart(string customerId);
    }
} 