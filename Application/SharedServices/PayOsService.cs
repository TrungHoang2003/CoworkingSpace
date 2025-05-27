using Domain.Entities;
using Domain.ResultPattern;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;

namespace Application.SharedServices;

public class PayOsService
{
    private readonly PayOS _payOs;
    private readonly string _returnUrl;
    private readonly string _cancelUrl;

    public PayOsService(IConfiguration configuration)
    {
        var clientId = configuration["PayOSSettings:ClientId"];
        var apiKey = configuration["PayOSSettings:ApiKey"];
        var checksumKey = configuration["PayOSSettings:ChecksumKey"];

        _payOs = new PayOS(clientId, apiKey, checksumKey);

        _returnUrl = configuration["PayOSSettings:ReturnUrl"];
        _cancelUrl = configuration["PayOSSettings:CancelUrl"];
    }

    public async Task<CreatePaymentResult> CreatePayment(Reservation reservation)
    {
        var expireAt = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds();
        var orderCode = reservation.ReservationId;
        var amount = 100; // fix cứng cho test, nhớ sửa sau
        var description = $"Thanh toan don hang {orderCode}";

        var items = new List<ItemData>();

        var paymentData = new PaymentData(
            orderCode, 
            amount, 
            description, 
            items, 
            _cancelUrl, 
            _returnUrl, 
            null, null, null, null, null, expireAt
        );

        var createPaymentResult = await _payOs.createPaymentLink(paymentData);
        return createPaymentResult;
    }

    public async Task ConfirmWebhook()
    {
        const string localUrl = "https://localhost:5196/PayOs/Webhook";
        await _payOs.confirmWebhook(localUrl);
    }
    
    public WebhookData VerifyPaymentWebHookData(WebhookType webhookBody)
    {
        var verifiedData = _payOs.verifyPaymentWebhookData(webhookBody);
        return verifiedData;
    }

    public async Task<PaymentLinkInformation> CancelPayment(Reservation reservation)
    {
        var paymentLinkInformation = await _payOs.cancelPaymentLink(reservation.ReservationId);
        return paymentLinkInformation;
    }
}
