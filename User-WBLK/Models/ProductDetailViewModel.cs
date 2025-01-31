using System.Globalization;

namespace Website_Ban_Linh_Kien.Models
{
    public class ProductDetailViewModel
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Specifications { get; set; }
        public List<string> AdditionalImages { get; set; }
        public string Brand { get; set; }
        public bool InStock { get; set; }
        public int Warranty { get; set; }
        public string ProductCode { get; set; }
        public int ViewCount { get; set; }
        public int PurchaseCount { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public List<ProductReviewViewModel> Reviews { get; set; } = new List<ProductReviewViewModel>();
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new Dictionary<int, int>();
        public int SoLuongTon { get; set; }
        public List<ProductCardViewModel> RelatedProducts { get; set; } = new List<ProductCardViewModel>();
    }
}