using Domain.Errors;
using Domain.ResultPattern;
using Domain.ViewModel;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.BookingWindows.CQRS.Queries;

public sealed record GetBookingWindowViewQuery(int VenueId) : IRequest<Result<List<BookingWindowListViewModel>>>;

public class
    GetBookingWindowViewQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetBookingWindowViewQuery, Result<List<BookingWindowListViewModel>>>
{
    public async Task<Result<List<BookingWindowListViewModel>>> Handle(GetBookingWindowViewQuery request,
        CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Venue.FindById(request.VenueId);
        if (!result) return Result<List<BookingWindowListViewModel>>.Failure(VenueErrors.VenueNotFound);
        
        var list = await unitOfWork.BookingWindow.GetByVenueId(request.VenueId);
        return Result<List<BookingWindowListViewModel>>.Success(list);
    }
}