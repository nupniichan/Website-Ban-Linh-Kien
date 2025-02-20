public class OrderConfirmationViewModel
{
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalTotal { get; set; }
    public decimal? VipDiscountPercentage { get; set; }
    public string? VipRank { get; set; }
}