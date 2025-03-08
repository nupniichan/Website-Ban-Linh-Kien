using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Admin_WBLK.Models.Commands
{
    public interface IProductCommand
    {
        Task<IActionResult> Execute();
    }
} 