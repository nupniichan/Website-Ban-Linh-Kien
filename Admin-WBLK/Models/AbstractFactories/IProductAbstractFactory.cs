using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Admin_WBLK.Models;

namespace Admin_WBLK.Models.AbstractFactories
{
    /// <summary>
    /// Interface định nghĩa Abstract Factory cho việc tạo các loại sản phẩm khác nhau
    /// </summary>
    public interface IProductAbstractFactory
    {
        /// <summary>
        /// Tạo sản phẩm điện tử
        /// </summary>
        Task<Sanpham> CreateElectronicProduct(
            string name, 
            decimal price, 
            int quantity, 
            string brand, 
            string description, 
            string specifications, 
            IFormFile imageFile);
            
        /// <summary>
        /// Tạo sản phẩm phụ kiện
        /// </summary>
        Task<Sanpham> CreateAccessoryProduct(
            string name, 
            decimal price, 
            int quantity, 
            string brand, 
            string description, 
            string specifications, 
            IFormFile imageFile);
            
        /// <summary>
        /// Tạo sản phẩm linh kiện
        /// </summary>
        Task<Sanpham> CreateComponentProduct(
            string name, 
            decimal price, 
            int quantity, 
            string brand, 
            string description, 
            string specifications, 
            IFormFile imageFile);
    }
} 