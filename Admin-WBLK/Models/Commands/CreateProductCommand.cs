using System;
using System.Threading.Tasks;
using Admin_WBLK.Models;
using Admin_WBLK.Models.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Admin_WBLK.Models.Commands
{
    public class CreateProductCommand
    {
        private readonly IProductFactory _productFactory;

        public CreateProductCommand(IProductFactory productFactory)
        {
            _productFactory = productFactory;
        }

        public async Task<Sanpham> Execute(
            string name,
            decimal price,
            int quantity,
            string brand,
            string description,
            string specifications,
            string category,
            IFormFile imageFile)
        {
            try
            {
                // Sử dụng Factory để tạo sản phẩm mới
                var newProduct = await _productFactory.CreateProduct(
                    name,
                    price,
                    quantity,
                    brand,
                    description,
                    specifications,
                    category,
                    imageFile
                );

                return newProduct;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateProductCommand - Exception: {ex.Message}");
                throw;
            }
        }
    }
} 