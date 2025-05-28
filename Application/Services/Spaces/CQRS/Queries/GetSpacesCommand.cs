using Domain.Entities;
using Domain.Filters;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Spaces.CQRS.Queries;

public sealed record GetSpacesCommand(SpaceFilter SpaceFilter):IRequest<Result<List<Space>>>;

public class GetSpacesCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetSpacesCommand, Result<List<Space>>>
{
    public async Task<Result<List<Space>>> Handle(GetSpacesCommand request, CancellationToken cancellationToken)
    {
        var spaces = await unitOfWork.Space.GetSpaces(request.SpaceFilter);
        return Result<List<Space>>.Success(spaces);
    }
}