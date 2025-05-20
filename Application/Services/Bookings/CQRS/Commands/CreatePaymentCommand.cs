using Application.SharedServices;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Bookings.CQRS.Commands;

public sealed record CreatePaymentCommand(int ReservationId):IRequest<Result<string>>;

public class CreatePaymentCommandHandler(IUnitOfWork unitOfWork, VnPayService vnPayService) : IRequestHandler<CreatePaymentCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var reservation = await unitOfWork.Reservation.GetById(request.ReservationId);
        if (reservation == null) return Result<string>.Failure(BookingErrors.ReservationNotFound);

        var paymentUrl = vnPayService.CreatePaymentUrl(reservation);

        return Result<string>.Success(paymentUrl);
    }
}

