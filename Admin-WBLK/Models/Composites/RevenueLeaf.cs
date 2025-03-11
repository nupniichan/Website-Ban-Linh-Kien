using System.Collections.Generic;

namespace Admin_WBLK.Models.Composites
{
    // Leaf class - đại diện cho một nguồn doanh thu đơn lẻ
    public class RevenueLeaf : IRevenueComponent
    {
        public string Name { get; private set; }
        public decimal Amount { get; private set; }
        public string Source { get; private set; }
        
        public RevenueLeaf(string name, decimal amount, string source)
        {
            Name = name;
            Amount = amount;
            Source = source;
        }
        
        public decimal Calculate()
        {
            return Amount;
        }
        
        public Dictionary<string, object> GetDetails()
        {
            return new Dictionary<string, object>
            {
                ["name"] = Name,
                ["amount"] = Amount,
                ["source"] = Source,
                ["type"] = "leaf"
            };
        }
    }
} 