using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Bookings.CQRS.Commands;

public sealed record Book(
    int SpaceId,
    DateTime StartDate,
    DateTime EndDate,
    int Capacity):IRequest<Result<BookResponse>>;

public sealed record BookResponse
{
    public int ReservationId { get; set; }
    public int SpaceId { get; set; }
    public int CustomerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Capacity{ get; set; }
    public int TotalDays { get; set; }
    public decimal TotalPrice { get; set; }
}

public class BookHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager) : 
    IRequestHandler<Book, Result<BookResponse>>
{
    public async Task<Result<BookResponse>> Handle(Book request, CancellationToken cancellationToken)
    {
        
        // Check if the space is booked or not
        var isBooked = await unitOfWork.Space.CheckAvailability(request.SpaceId, request.StartDate, request.EndDate);
        if (isBooked) return Result<BookResponse>.Failure(BookingErrors.SpaceBookedAtThisTime);
        
        // Check if the space exists
        var space = await unitOfWork.Space.GetById(request.SpaceId);
        if (space is null) return Result<BookResponse>.Failure(SpaceErrors.SpaceNotFound);
        
        var totalDays = (request.EndDate.Date - request.StartDate.Date).Days + 1;
        if (totalDays <= 30 && space.ListingType == ListingType.MonthOnly)
            return Result<BookResponse>.Failure(BookingErrors.InvalidTime);

        var venue = await unitOfWork.Venue.GetById(space.VenueId);
        if (venue is null) return Result<BookResponse>.Failure(VenueErrors.VenueNotFound);
        
        // get UserId from context
        var userId = httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
        var user = await userManager.FindByIdAsync(userId!);
        if (user == null) return Result<BookResponse>.Failure(AuthenErrors.NotLoggedIn);
        
        var price = await unitOfWork.Price.GetById(space.PriceId);
        if (price == null) return Result<BookResponse>.Failure(SpaceErrors.PriceNotFound);
        var reservation = new Reservation
        {
            CustomerId = Convert.ToInt32(userId),
            SpaceId = request.SpaceId,
            Capacity = request.Capacity,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            SalesPrice = price.Amount * totalDays,
            TotalPrice = price.Amount * totalDays,
        };
        
        await unitOfWork.Reservation.Create(reservation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new BookResponse
        {
            ReservationId = reservation.ReservationId,
            SpaceId = reservation.SpaceId,
            CustomerId = reservation.CustomerId,
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
            TotalDays = totalDays,
            Capacity = request.Capacity,
            TotalPrice = reservation.TotalPrice
        };

        return Result<BookResponse>.Success(response);
    }
}