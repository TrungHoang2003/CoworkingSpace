using Application.DTOs;
using AutoMapper;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Repositories;
using MediatR;

namespace Application.BookingWindowService;

public sealed record UpdateBookingWindowCommand(UpdateBookingWindowRequest UpdateBookingWindowRequest): IRequest<Result>;

public class UpdateBookingWindowCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateBookingWindowCommand, Result>
{
    public async Task<Result> Handle(UpdateBookingWindowCommand command, CancellationToken cancellationToken)
    {
        var request = command.UpdateBookingWindowRequest;
        //Validate du lieu tu request
        request.Validate();

        var existingBookingWindow = await unitOfWork.BookingWindow.GetById(request.BookingWindowId);
        if (existingBookingWindow is null) return BookingWindowErrors.BookingWindowNotFound;
        
        //Update booking window
        mapper.Map(request, existingBookingWindow); 
        await unitOfWork.BookingWindow.Update(existingBookingWindow);
        
        return Result.Success();
    }
}