using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.SharedServices;

public class VnPayService(IConfiguration configuration)
{
    public string CreatePaymentUrl(HttpContext httpContext, decimal totalAmount, string orderId)
    {
        var vnPayUrl = configuration["VnPay:PaymentUrl"];
        var returnUrl = configuration["VnPay:ReturnUrl"];
        var tmnCode = configuration["VnPay:TmnCode"];
        var hashSecret = configuration["VnPay:HashSecret"];

        var payParams = new SortedDictionary<string, string>
        {
            { "vnp_Version", "2.1.0" },
            { "vnp_Command", "pay" },
            { "vnp_TmnCode", tmnCode },
            { "vnp_Amount", ((int)(totalAmount * 100)).ToString() },
            { "vnp_CurrCode", "VND" },
            { "vnp_TxnRef", orderId },
            { "vnp_OrderInfo", $"Thanh toan don hang {orderId}" },
            { "vnp_OrderType", "other" },
            { "vnp_Locale", "vn" },
            { "vnp_ReturnUrl", returnUrl },
            { "vnp_IpAddr", httpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1" },
            { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
        };

        var queryString = BuildQueryString(payParams, hashSecret);

        return $"{vnPayUrl}?{queryString}";
    }

    public bool ValidateReturnData(IQueryCollection queryCollection)
    {
        var hashSecret = configuration["VnPay:HashSecret"];
        var inputData = new SortedDictionary<string, string>();

        foreach (var (key, value) in queryCollection)
        {
            if (key.StartsWith("vnp_") && key != "vnp_SecureHash" && key != "vnp_SecureHashType")
            {
                inputData.Add(key, value);
            }
        }

        var checkSum = queryCollection["vnp_SecureHash"];
        var rawData = string.Join("&", inputData.Select(kv => $"{kv.Key}={kv.Value}"));

        var computedHash = ComputeSha256Hash(hashSecret + rawData);

        return string.Equals(checkSum, computedHash, StringComparison.InvariantCultureIgnoreCase);
    }

    private string BuildQueryString(SortedDictionary<string, string> inputData, string hashSecret)
    {
        var query = string.Join("&", inputData.Select(kv => $"{kv.Key}={HttpUtility.UrlEncode(kv.Value)}"));
        var signData = string.Join("&", inputData.Select(kv => $"{kv.Key}={kv.Value}"));
        var secureHash = ComputeSha256Hash(hashSecret + signData);

        return $"{query}&vnp_SecureHashType=SHA256&vnp_SecureHash={secureHash}";
    }

    private string ComputeSha256Hash(string rawData)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));
        var builder = new StringBuilder();
        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }
}