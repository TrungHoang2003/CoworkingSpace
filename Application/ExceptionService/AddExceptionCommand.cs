using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.Repositories;
using MediatR;

namespace Application.ExceptionService;

public sealed record AddExceptionCommand(AddExceptionRequest AddExceptionRequest) : IRequest<Result>;

public class AddExceptionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AddExceptionCommand, Result>
{
    public async Task<Result> Handle(AddExceptionCommand command, CancellationToken cancellationToken)
    {
        var request = command.AddExceptionRequest;
        request.Validate();

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
                return Result.Failure(new Error("Space Error", "Cannot find space with Id = " + spaceId + ""));

            space.Exception = exceptionRule;
            await unitOfWork.Space.Update(space);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}