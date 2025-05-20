using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace Application.SharedServices;

public class VnPayService(IConfiguration configuration)
{
    public string CreatePaymentUrl(Reservation reservation)
    {
        var vnpUrl = configuration["VnPay:PaymentUrl"];
        var vnpReturnUrl = configuration["VnPay:ReturnUrl"];
        var vnpTmnCode = configuration["VnPay:TmnCode"];
        var vnpHashSecret = configuration["VnPay:HashSecret"];

        var amount = (reservation.TotalPrice ?? 0) * 100; // nhân 100 vì VNPAY tính bằng VND * 100
        var txnRef = reservation.ReservationId.ToString();
        var createDate = DateTime.Now.ToString("yyyyMMddHHmmss");

        var param = new SortedDictionary<string, string>
        {
            {"vnp_Version", "2.1.0"},
            {"vnp_Command", "pay"},
            {"vnp_TmnCode", vnpTmnCode},
            {"vnp_Amount", ((long)amount).ToString()},
            {"vnp_CreateDate", createDate},
            {"vnp_CurrCode", "VND"},
            {"vnp_IpAddr", "127.0.0.1"},
            {"vnp_Locale", "vn"},
            {"vnp_OrderInfo", "Thanh toan don hang #" + txnRef},
            {"vnp_OrderType", "other"},
            {"vnp_ReturnUrl", vnpReturnUrl},
            {"vnp_TxnRef", txnRef}
        };

        var queryString = QueryHelpers.AddQueryString("", param);

        var signData = string.Join('&', param.Select(x => $"{x.Key}={x.Value}"));
        var sign = HmacSHA512(vnpHashSecret, signData);

        var paymentUrl = $"{vnpUrl}?{queryString}&vnp_SecureHash={sign}";
        return paymentUrl;
    }

    private string HmacSHA512(string key, string inputData)
    {
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        return hashString;
    }
}