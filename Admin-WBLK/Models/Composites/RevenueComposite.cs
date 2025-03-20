using System.Collections.Generic;
using System.Linq;

namespace Admin_WBLK.Models.Composites
{
    // Composite class - đại diện cho một nhóm các nguồn doanh thu
    public class RevenueComposite : IRevenueComponent
    {
        public string Name { get; private set; }
        private List<IRevenueComponent> _children = new List<IRevenueComponent>();
        
        public RevenueComposite(string name)
        {
            Name = name;
        }
        
        public void Add(IRevenueComponent component)
        {
            _children.Add(component);
        }
        
        public void Remove(IRevenueComponent component)
        {
            _children.Remove(component);
        }
        
        public decimal Calculate()
        {
            return _children.Sum(c => c.Calculate());
        }
        
        public Dictionary<string, object> GetDetails()
        {
            return new Dictionary<string, object>
            {
                ["name"] = Name,
                ["amount"] = Calculate(),
                ["children"] = _children.Select(c => c.GetDetails()).ToList(),
                ["type"] = "composite"
            };
        }
    }
} 