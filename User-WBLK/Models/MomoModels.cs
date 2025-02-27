using Newtonsoft.Json;

namespace Website_Ban_Linh_Kien.Models
{
    public class MomoCreatePaymentRequest
    {
        [JsonProperty("partnerCode")]
        public string PartnerCode { get; set; }
        
        [JsonProperty("accessKey")]
        public string AccessKey { get; set; }
        
        [JsonProperty("requestId")]
        public string RequestId { get; set; }
        
        [JsonProperty("amount")]
        public long Amount { get; set; }
        
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        
        [JsonProperty("orderInfo")]
        public string OrderInfo { get; set; }
        
        [JsonProperty("redirectUrl")]
        public string ReturnUrl { get; set; }
        
        [JsonProperty("ipnUrl")]
        public string NotifyUrl { get; set; }
        
        [JsonProperty("requestType")]
        public string RequestType { get; set; }
        
        [JsonProperty("signature")]
        public string Signature { get; set; }
        
        [JsonProperty("extraData")]
        public string ExtraData { get; set; }
    }

    public class MomoCreatePaymentResponse
    {
        [JsonProperty("requestId")]
        public string RequestId { get; set; }
        
        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }
        
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("localMessage")]
        public string LocalMessage { get; set; }
        
        [JsonProperty("requestType")]
        public string RequestType { get; set; }
        
        [JsonProperty("payUrl")]
        public string PayUrl { get; set; }
        
        [JsonProperty("signature")]
        public string Signature { get; set; }
        
        [JsonProperty("qrCodeUrl")]
        public string QrCodeUrl { get; set; }
        
        [JsonProperty("deeplink")]
        public string Deeplink { get; set; }
        
        [JsonProperty("deeplinkMiniApp")]
        public string DeeplinkMiniApp { get; set; }
    }

    public class MomoIPN
    {
        [JsonProperty("partnerCode")]
        public string PartnerCode { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("orderInfo")]
        public string OrderInfo { get; set; }

        [JsonProperty("orderType")]
        public string OrderType { get; set; }

        [JsonProperty("transId")]
        public string TransId { get; set; }

        [JsonProperty("resultCode")]
        public int ResultCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("payType")]
        public string PayType { get; set; }

        [JsonProperty("responseTime")]
        public long ResponseTime { get; set; }

        [JsonProperty("extraData")]
        public string ExtraData { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
} 