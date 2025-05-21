using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Bookings.CQRS.Commands;

public sealed record DailySpaceBookCommand(
    int SpaceId,
    DateTime StartDate,
    DateTime EndDate,
    int Quantity):IRequest<Result<DailySpaceBookResponse>>;

public sealed record DailySpaceBookResponse
{
    public int ReservationId { get; set; }
    public int SpaceId { get; set; }
    public int CustomerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? Quantity { get; set; }
    public int? TotalDays { get; set; }
    public decimal TotalPrice { get; set; }
}

public class DailySpaceBookCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager) : 
    IRequestHandler<DailySpaceBookCommand, Result<DailySpaceBookResponse>>
{
    public async Task<Result<DailySpaceBookResponse>> Handle(DailySpaceBookCommand request, CancellationToken cancellationToken)
    {
        // Check if the space exists
        var space = await unitOfWork.Space.GetById(request.SpaceId);
        if (space is null) return Result<DailySpaceBookResponse>.Failure(SpaceErrors.SpaceNotFound);

        // Check if the space is available
        if (space.Quantity < request.Quantity)
            return Result<DailySpaceBookResponse>.Failure(BookingErrors.SpaceNotAvailable);
        
        var venue = await unitOfWork.Venue.GetById(space.VenueId);
        if (venue is null) return Result<DailySpaceBookResponse>.Failure(VenueErrors.VenueNotFound);
        
        // Check if the space is closed on the requested date
        var closedDays = await unitOfWork.Venue.GetClosedDays(venue.VenueId);
        for (var date = request.StartDate; date <= request.EndDate; date = date.AddDays(1))
        {
            if (closedDays.Contains(date.DayOfWeek))
                return Result<DailySpaceBookResponse>.Failure(new Error("Lỗi đặt phòng", "Phòng đóng cửa vào ngày " + date.ToString("dd/MM/yyyy")));
        }
        
        // get UserId from context
        var userId = httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
        var user = await userManager.FindByIdAsync(userId!);
        if (user == null) return Result<DailySpaceBookResponse>.Failure(AuthenErrors.NotLoggedIn);
        
        var totalDays = (request.EndDate.Date - request.StartDate.Date).Days + 1;

        var price = await unitOfWork.Price.GetById(space.PriceId);
        if (price == null) return Result<DailySpaceBookResponse>.Failure(SpaceErrors.PriceNotFound);
        var reservation = new Reservation
        {
            CustomerId = Convert.ToInt32(userId),
            SpaceId = request.SpaceId,
            Quantity = request.Quantity,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            SalesPrice = price.Amount * request.Quantity * totalDays,
            TotalPrice = price.Amount * request.Quantity * totalDays,
        };
        
        await unitOfWork.Reservation.Create(reservation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new DailySpaceBookResponse
        {
            ReservationId = reservation.ReservationId,
            SpaceId = reservation.SpaceId,
            CustomerId = reservation.CustomerId,
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
            Quantity = reservation.Quantity,
            TotalDays = totalDays,
            TotalPrice = reservation.TotalPrice ?? 0
        };

        return Result<DailySpaceBookResponse>.Success(response);
    }
}