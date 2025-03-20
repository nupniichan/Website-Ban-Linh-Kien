using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Website_Ban_Linh_Kien.Models.Commands.Cart
{
    public interface ICartCommand
    {
        Task<IActionResult> Execute();
    }
} 