using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Services
{
    public class MomoService : IMomoService
    {
        private readonly MomoOptionModel _options;
        private readonly IHttpClientFactory _httpClientFactory;

        public MomoService(IOptions<MomoOptionModel> options, IHttpClientFactory httpClientFactory)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<MomoCreatePaymentResponse> CreatePaymentAsync(long amount, string orderId, string orderInfo, string extraData = "", string requestType = null)
        {
            var requestId = DateTime.UtcNow.Ticks.ToString();
            
            // Sử dụng requestType từ tham số hoặc từ cấu hình
            string actualRequestType = requestType ?? _options.RequestType;
            
            // Tạo request object theo định dạng mới của Momo API v2
            var request = new MomoCreatePaymentRequest
            {
                PartnerCode = _options.PartnerCode,
                AccessKey = _options.AccessKey,
                RequestId = requestId,
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo,
                ReturnUrl = _options.ReturnUrl,
                NotifyUrl = _options.NotifyUrl,
                RequestType = actualRequestType,
                ExtraData = extraData
            };
            
            // Tạo chuỗi hash theo định dạng mới của Momo API v2
            var rawHash = $"accessKey={request.AccessKey}" +
                         $"&amount={request.Amount}" +
                         $"&extraData={request.ExtraData}" +
                         $"&ipnUrl={request.NotifyUrl}" +
                         $"&orderId={request.OrderId}" +
                         $"&orderInfo={request.OrderInfo}" +
                         $"&partnerCode={request.PartnerCode}" +
                         $"&redirectUrl={request.ReturnUrl}" +
                         $"&requestId={request.RequestId}" +
                         $"&requestType={request.RequestType}";

            // Tạo chữ ký
            var signature = ComputeHmacSha256(rawHash, _options.SecretKey);
            request.Signature = signature;

            var client = _httpClientFactory.CreateClient();
            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Gọi API và xử lý response
            var response = await client.PostAsync(_options.MomoApiUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            // Log response để debug
            Console.WriteLine($"Momo API Response: {responseContent}");
            
            return JsonConvert.DeserializeObject<MomoCreatePaymentResponse>(responseContent);
        }

        public bool ValidateSignature(string rawHash, string signature)
        {
            var hash = ComputeHmacSha256(rawHash, _options.SecretKey);
            return hash.Equals(signature, StringComparison.OrdinalIgnoreCase);
        }

        public string GetAccessKey()
        {
            return _options.AccessKey;
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var key = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmac = new HMACSHA256(key))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
} 