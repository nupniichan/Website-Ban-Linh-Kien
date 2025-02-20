using System.ComponentModel.DataAnnotations;
using System.Linq;

public class CheckoutViewModel
{
    public string? CustomerId { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập tên người nhận")]
    [Display(Name = "Tên người nhận")]
    public string ReceiverName { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [Display(Name = "Số điện thoại")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string ReceiverPhone { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; }

    [Display(Name = "Ghi chú")]
    public string Note { get; set; } = null;

    [Display(Name = "Địa chỉ cửa hàng")]
    public string StoreAddress { get; set; } = "123 Nguyễn Văn A, Quận 1, TP.HCM";

    public DeliveryMethod DeliveryMethod { get; set; }

    public string? StreetAddress { get; set; }
    public string? Ward { get; set; }
    public string? District { get; set; }
    public string? City { get; set; }

    public List<CheckoutItemViewModel> Items { get; set; } = new List<CheckoutItemViewModel>();

    [Display(Name = "Tổng tiền")]
    public decimal TotalAmount => Items.Sum(item => item.SubTotal);
}

public class CheckoutItemViewModel
{
    public string ProductId { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SubTotal => Price * Quantity;
}

public enum DeliveryMethod
{
    [Display(Name = "Nhận tại cửa hàng")]
    StorePickup,
    
    [Display(Name = "Giao hàng tận nơi")]
    HomeDelivery
} 