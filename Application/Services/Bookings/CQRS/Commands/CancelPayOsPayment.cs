using Application.SharedServices;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Bookings.CQRS.Commands;

public sealed record CancelPayOsPayment(int ReservationId) : IRequest<Result>;

public class CancelPayOsPaymentHandler(PayOsService payOsService, IUnitOfWork unitOfWork)
    : IRequestHandler<CancelPayOsPayment, Result>
{
    public async Task<Result> Handle(CancelPayOsPayment request, CancellationToken cancellationToken)
    {
        var reservation = await unitOfWork.Reservation.GetById(request.ReservationId);
        if (reservation == null) return BookingErrors.ReservationNotFound;
        
        var result = await payOsService.CancelPayment(reservation);
        reservation.Status = result.status;
        await unitOfWork.Reservation.Update(reservation);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}