using System.Collections.Generic;

public class ProductListViewModel
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string Category { get; set; }
    public string Brand { get; set; }
    public string PriceRange { get; set; }
    public string Price { get; set; }
    public Dictionary<string, string> AdditionalFilters { get; set; } = new Dictionary<string, string>();
    public string Size { get; set; }
    public string Resolution { get; set; }
    public string RefreshRate { get; set; }
    public string Capacity { get; set; }
    public string Type { get; set; }
    public string Connection { get; set; }
    public string Features { get; set; }
    public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
}