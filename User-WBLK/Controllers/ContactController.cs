using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Website_Ban_Linh_Kien.Models;
using System.Linq;
using System.Collections.Generic;

namespace Website_Ban_Linh_Kien.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}