using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Admin_WBLK.Models.Commands
{
    public interface ICommand
    {
        Task<IActionResult> Execute();
    }
}
