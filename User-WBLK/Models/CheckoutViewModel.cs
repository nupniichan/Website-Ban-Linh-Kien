using System.ComponentModel.DataAnnotations;

public class CheckoutViewModel
{
    public DeliveryMethod DeliveryMethod { get; set; }
    public string StoreAddress { get; set; }
    
    [Display(Name = "Tên người nhận")]
    [Required(ErrorMessage = "Vui lòng nhập tên người nhận")]
    public string ReceiverName { get; set; }
    
    [Display(Name = "SĐT người nhận")]
    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
    public string ReceiverPhone { get; set; }
    
    [Display(Name = "Tỉnh/Thành phố")]
    [Required(ErrorMessage = "Vui lòng chọn tỉnh/thành phố")]
    public string City { get; set; }
    
    [Display(Name = "Quận/Huyện")]
    [Required(ErrorMessage = "Vui lòng chọn quận/huyện")]
    public string District { get; set; }
    
    [Display(Name = "Phường/Xã")]
    [Required(ErrorMessage = "Vui lòng chọn phường/xã")]
    public string Ward { get; set; }
    
    [Display(Name = "Số nhà, tên đường")]
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ chi tiết")]
    public string StreetAddress { get; set; }
    
    [Display(Name = "Ghi chú")]
    public string Note { get; set; }
}

public enum DeliveryMethod
{
    [Display(Name = "Nhận tại cửa hàng")]
    StorePickup,
    
    [Display(Name = "Giao hàng tận nơi")]
    HomeDelivery
} 