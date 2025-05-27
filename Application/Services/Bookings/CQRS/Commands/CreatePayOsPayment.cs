using Application.SharedServices;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Bookings.CQRS.Commands;

public sealed record CreatePayOsPayment(int ReservationId): IRequest<Result<string>>;

public class CreatePayOsPaymentHandler(IUnitOfWork unitOfWork, PayOsService payOsService)
    : IRequestHandler<CreatePayOsPayment, Result<string>>
{
    public async Task<Result<string>> Handle(CreatePayOsPayment request, CancellationToken cancellationToken)
    {
        var reservation = await unitOfWork.Reservation.GetById(request.ReservationId);
        if (reservation == null) return Result<string>.Failure(BookingErrors.ReservationNotFound);
        var paymentResult = await payOsService.CreatePayment(reservation);
        reservation.Status = paymentResult.status;
        await unitOfWork.Reservation.Update(reservation);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var url = paymentResult.checkoutUrl;
        return Result<string>.Success(url);
    }
}