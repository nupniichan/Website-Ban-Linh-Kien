using System.Threading.Tasks;
using Admin_WBLK.Models;
using Microsoft.AspNetCore.Http;

namespace Admin_WBLK.Models.Factories
{
    public interface IProductFactory
    {
        Task<Sanpham> CreateProduct(
            string name, 
            decimal price, 
            int quantity, 
            string brand, 
            string description, 
            string specifications, 
            string category, 
            IFormFile imageFile);
        
        Task<string> GenerateProductId();
        Task<string> ProcessImage(IFormFile imageFile);
    }
} 