using Application.Services.Bookings.CQRS.Commands;
using Application.SharedServices;
using Domain.Errors;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class PayOsController(PayOsService payOsService, IMediator mediator, IReservationRepository reservationRepo):Controller
{
    [HttpPost("ConfirmWebHook")]
    public async Task<IActionResult> ConfirmWebHook()
    {
        await payOsService.ConfirmWebhook();
        return Ok("Webhook confirmed");
    }
    
    [HttpPost("Webhook")]
    private async Task<IActionResult> WebHook([FromBody] WebhookType webhookBody)
    {
        var verifiedData = payOsService.VerifyPaymentWebHookData(webhookBody);
        var reservation = await reservationRepo.GetById((int)verifiedData.orderCode);
        if (reservation == null) return BadRequest(PayOsErrors.ReservationNotFound);
        reservation.Status = verifiedData.code;
        await reservationRepo.Update(reservation);
        await reservationRepo.SaveChangesAsync();
        return Ok("Webhook data verified and reservation updated");
    }
    
    [HttpPost("CreatePayment")]
    public async Task<IActionResult> CreatePayOsPayment([FromBody] CreatePayOsPayment command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }
    
    [HttpPost("CancelPayment")]
    public async Task<IActionResult> CancelPayOsPayment([FromBody] CancelPayOsPayment command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result);
    }
}