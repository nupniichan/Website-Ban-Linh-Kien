using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Website_Ban_Linh_Kien.Models.Strategies.Cart
{
    public interface ICartStrategy
    {
        Task<IActionResult> Execute(DatabaseContext context, string customerId, object data);
    }
} 