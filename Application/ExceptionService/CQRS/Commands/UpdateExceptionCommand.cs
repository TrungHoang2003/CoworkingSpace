using Application.ExceptionService.Mappings;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.ExceptionService.CQRS.Commands;

public sealed record UpdateExceptionCommand(
    int ExceptionId,
    int VenueId,
    ExceptionUnit Unit,
    DateTime? StartDate,
    DateTime? EndDate,
    TimeSpan? StartTime,
    TimeSpan? EndTime,
    string? Description,
    bool ApplyAll,
    List<DayOfWeek>? Days,
    List<int>? SpaceIds     
    ) : IRequest<Result>;

public class UpdateExceptionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateExceptionCommand, Result>
{
    public async Task<Result> Handle(UpdateExceptionCommand request, CancellationToken cancellationToken)
    {
        var exception = await unitOfWork.Exception.GetById(request.ExceptionId);
        if (exception == null) return ExceptionErrors.ExceptionNotFound;
        
        exception = request.ToExceptionRule(exception);
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
        else
        {
            foreach (var spaceId in request.SpaceIds!)
            {
                var space = await unitOfWork.Space.GetById(spaceId);
                if (space == null)
                    return Result.Failure(new Error("Space Error", "Cannot find space with Id = " + spaceId + ""));

                space.Exception = exception;
                await unitOfWork.Space.Update(space);
            }
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
    