using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Web;
using Microsoft.Extensions.Logging;

namespace Application.SharedServices;

public class VnPayService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<VnPayService> logger)
{
    public string CreatePaymentUrl(Reservation reservation)
    {
        var vnpUrl = configuration["VnPay:PaymentUrl"];
        var vnpReturnUrl = configuration["VnPay:ReturnUrl"];
        var vnpTmnCode = configuration["VnPay:TmnCode"];
        var vnpHashSecret = configuration["VnPay:HashSecret"];

        // Kiểm tra vnpHashSecret
        if (string.IsNullOrEmpty(vnpHashSecret))
        {
            logger.LogError("vnpHashSecret is empty or null");
            throw new InvalidOperationException("vnpHashSecret is not configured");
        }
        logger.LogInformation("vnpHashSecret (first 5 chars): {HashSecret}", vnpHashSecret.Substring(0, Math.Min(5, vnpHashSecret.Length)));

        // Chuyển TotalPrice thành số nguyên
        var amount = (long)(reservation.TotalPrice  * 100);
        var txnRef = reservation.ReservationId.ToString();
        var createDate = DateTime.Now.ToString("yyyyMMddHHmmss");

        // Lấy IP Address
        var ipAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1" || ipAddress == "127.0.0.1")
        {
            ipAddress = "203.0.113.1"; // Giả lập IP công khai
            logger.LogWarning("Localhost IP detected. Using fallback IP: {IpAddress}", ipAddress);
        }
        logger.LogInformation("IP Address: {IpAddress}", ipAddress);

        // Tạo danh sách tham số
        var param = new SortedDictionary<string, string>(StringComparer.Ordinal)
        {
            { "vnp_Amount", amount.ToString() },
            { "vnp_Command", "pay" },
            { "vnp_CreateDate", createDate },
            { "vnp_CurrCode", "VND" },
            { "vnp_IpAddr", ipAddress },
            { "vnp_Locale", "vn" },
            { "vnp_OrderInfo", $"Thanh toan don hang:{txnRef}" },
            { "vnp_OrderType", "other" },
            { "vnp_ReturnUrl", vnpReturnUrl },
            { "vnp_TmnCode", vnpTmnCode },
            { "vnp_TxnRef", txnRef },
            { "vnp_Version", "2.1.0" }
        };

        // Tạo chuỗi signData
        var signData = new StringBuilder();
        foreach (var entry in param)
        {
            var encodedValue = Uri.EscapeDataString(entry.Value);
            signData.Append($"{entry.Key}={encodedValue}&");
            logger.LogDebug("Adding to signData: {Key}={Value}", entry.Key, encodedValue);
        }
        if (signData.Length > 0)
        {
            signData.Length--;
        }

        logger.LogInformation("signData: {SignData}", signData.ToString());

        // Tạo chữ ký
        var sign = HmacSHA512(vnpHashSecret, signData.ToString());
        logger.LogInformation("vnp_SecureHash: {SecureHash}", sign);

        // Tạo URL thanh toán
        var queryString = new StringBuilder();
        foreach (var entry in param)
        {
            var encodedValue = Uri.EscapeDataString(entry.Value);
            queryString.Append($"{entry.Key}={encodedValue}&");
        }
        queryString.Append($"vnp_SecureHash={sign}");

        var paymentUrl = $"{vnpUrl}?{queryString}";
        logger.LogInformation("Payment URL: {PaymentUrl}", paymentUrl);
        return paymentUrl;
    }

    private string HmacSHA512(string key, string inputData)
    {
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}