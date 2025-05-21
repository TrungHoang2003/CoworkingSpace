using Application.SharedServices;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Bookings.CQRS.Commands;

public sealed record CreatePayOsPaymentCommand(int ReservationId): IRequest<Result<string>>;

public class CreatePayOsPaymentCommandHandler(IUnitOfWork unitOfWork, PayOsService payOsService)
    : IRequestHandler<CreatePayOsPaymentCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreatePayOsPaymentCommand request, CancellationToken cancellationToken)
    {
        var reservation = await unitOfWork.Reservation.GetById(request.ReservationId);
        if (reservation == null) return Result<string>.Failure(BookingErrors.ReservationNotFound);
        var paymentUrl = await payOsService.CreatePayment(reservation);
        return Result<string>.Success(paymentUrl);
    }
}