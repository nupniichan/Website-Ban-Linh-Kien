using Website_Ban_Linh_Kien.Models;

namespace Website_Ban_Linh_Kien.Services
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponse> CreatePaymentAsync(long amount, string orderId, string orderInfo, string extraData = "", string requestType = null);
        bool ValidateSignature(string rawHash, string signature);
        string GetAccessKey();
    }
} 