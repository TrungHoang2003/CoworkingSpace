using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Spaces.CQRS.Queries;

public sealed record GetSuggestionsCommand(string Keyword) : IRequest<Result<List<string>>>;

public class GetSuggestionsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetSuggestionsCommand, Result<List<string>>>
{
    public async Task<Result<List<string>>> Handle(GetSuggestionsCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Keyword) || request.Keyword.Length < 2)
        {
            return Result<List<string>>.Success([]);
        }

        var suggestions = await unitOfWork.Space.GetSearchSuggestions(request.Keyword);
        return Result<List<string>>.Success(suggestions);
    }
}
