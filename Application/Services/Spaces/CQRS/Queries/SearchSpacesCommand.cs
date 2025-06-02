using Domain.Entities;
using Domain.Filters;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Spaces.CQRS.Queries;

public sealed record SearchSpacesCommand(string searchTerm) : IRequest<Result<List<Space>>>;

public class SearchSpacesCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SearchSpacesCommand, Result<List<Space>>>
{
    public async Task<Result<List<Space>>> Handle(SearchSpacesCommand request, CancellationToken cancellationToken)
    {
        var spaces = await unitOfWork.Space.SearchSpaces(request.searchTerm);
        return Result<List<Space>>.Success(spaces);
    }
}
