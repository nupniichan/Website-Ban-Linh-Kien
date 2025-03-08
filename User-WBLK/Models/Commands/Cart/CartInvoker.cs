using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Website_Ban_Linh_Kien.Models.Commands.Cart
{
    public class CartInvoker
    {
        private ICartCommand _command;

        public void SetCommand(ICartCommand command)
        {
            _command = command;
        }

        public async Task<IActionResult> ExecuteCommand()
        {
            return await _command.Execute();
        }
    }
} 