using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Repositories;
using MediatR;

namespace Application.ExceptionService;

public sealed record UpdateExceptionCommand(UpdateExceptionRequest UpdateExceptionRequest) : IRequest<Result>;

public class UpdateExceptionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateExceptionCommand, Result>
{
    public async Task<Result> Handle(UpdateExceptionCommand command, CancellationToken cancellationToken)
    {
        var request = command.UpdateExceptionRequest;
        request.Validate();

        var exception = await unitOfWork.Exception.GetById(request.ExceptionId);
        if (exception == null) return ExceptionErrors.ExceptionNotFound;
        
        mapper.Map(exception, request);
        await unitOfWork.Exception.Update(exception);
        
        if (request.ApplyAll)
        {
            var spaces = await unitOfWork.Space.GetVenueWorkingSpacesAsync(request.VenueId);
            foreach (var space in spaces)
            {
                space.Exception = exception;
                await unitOfWork.Space.Update(space);
            }
        }

        foreach (var spaceId in request.SpaceIds!)
        {
            var space = await unitOfWork.Space.GetById(spaceId);
            if (space == null)
                return Result.Failure(new Error("Space Error", "Cannot find space with Id = " + spaceId + ""));

            space.Exception = exception;
            await unitOfWork.Space.Update(space);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
    