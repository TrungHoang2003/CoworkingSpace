using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;

namespace Application.SharedServices;

public class PayOsService(IConfiguration configuration)
{
    public async Task<string> CreatePayment(Reservation reservation)
    {
        var clientId = configuration["PayOSSettings:ClientId"];
        var checksumKey = configuration["PayOSSettings:ChecksumKey"];
        var apiKey = configuration["PayOSSettings:ApiKey"];
        var returnUrl = configuration["PayOSSettings:ReturnUrl"];
        var cancelUrl = configuration["PayOSSettings:CancelUrl"];
        var orderCode = reservation.ReservationId;
        var amount = (int)((reservation.TotalPrice ?? 0) * 100);
        var description = "Thanh toan don hang " + orderCode;

        var items = new List<ItemData>();
        var payOs = new PayOS(clientId, apiKey, checksumKey);
        
        var paymentData = new PaymentData(orderCode, amount, description, items, cancelUrl, returnUrl);
        var createPayment = await payOs.createPaymentLink(paymentData);
        return createPayment.ToString();
    }
}