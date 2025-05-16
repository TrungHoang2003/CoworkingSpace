using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Application.ExceptionService.Mappings;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using FluentValidation;
using Infrastructure.Repositories;
using MediatR;

namespace Application.ExceptionService.CQRS.Commands;

public sealed record AddExceptionCommand(
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

public class AddExceptionCommandHandler(IUnitOfWork unitOfWork, IValidator<AddExceptionCommand> validator) : IRequestHandler<AddExceptionCommand, Result>
{
    public async Task<Result> Handle(AddExceptionCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if(!validatorResult.IsValid)
            return Result.Failure(new Error("Validation Errors", string.Join("; ",validatorResult.Errors.Select(x => x.ErrorMessage).ToList())));
        
        var exist = await unitOfWork.Venue.FindById(request.VenueId);
        if (!exist) return VenueErrors.VenueNotFound;

        var exceptionRule = request.ToExceptionRule();
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
        else
        {
            foreach (var spaceId in request.SpaceIds!)
            {
                var space = await unitOfWork.Space.GetById(spaceId);
                if (space == null)
                    return Result.Failure(new Error("Space Error", "Cannot find space with Id = " + spaceId + ""));

                space.Exception = exceptionRule;
                await unitOfWork.Space.Update(space);
            }
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}