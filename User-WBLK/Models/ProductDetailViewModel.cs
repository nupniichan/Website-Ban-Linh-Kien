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
        public List<ProductReview> Reviews { get; set; }
    }

    public class ProductReview
    {
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}