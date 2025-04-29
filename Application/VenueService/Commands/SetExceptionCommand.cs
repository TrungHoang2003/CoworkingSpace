using Application.ExceptionService.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Repositories;
using MediatR;

namespace Application.VenueService.Commands;

public sealed record SetExceptionCommand(SetUpExceptionRequest SetUpExceptionRequest) : IRequest<Result>;

public class SetExceptionRuleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<SetExceptionCommand, Result>
{
    public async Task<Result> Handle(SetExceptionCommand command, CancellationToken cancellationToken)
    {
        var request = command.SetUpExceptionRequest;
        request.Validate();
        
        var venue = await unitOfWork.Venue.GetById(request.VenueId);
        if (venue == null)
            return VenueErrors.VenueNotFound;
        
        var exceptionRule = mapper.Map<ExceptionRule>(request);
        await unitOfWork.Exception.Create(exceptionRule);
        
        if (request.ApplyAll)
        {
            var spaces = await unitOfWork.Space.GetVenueWorkingSpacesAsync(request.VenueId);
            foreach (var space in spaces)
            {
                space.Exception = exceptionRule;
                await unitOfWork.Space.Update(space);
            }
        }

        foreach (var spaceId in request.SpaceIds!)
        {
            var space = await unitOfWork.Space.GetByIdAndVenue(spaceId, request.VenueId);
            if (space == null)
                return SpaceErrors.SpaceNotFound;
            
            space.Exception = exceptionRule;
            await unitOfWork.Space.Update(space);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}