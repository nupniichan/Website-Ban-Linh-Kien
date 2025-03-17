using System.Collections.Generic;

namespace Admin_WBLK.Models.Composites
{
    // Component interface
    public interface IRevenueComponent
    {
        string Name { get; }
        decimal Calculate();
        Dictionary<string, object> GetDetails();
    }
} 