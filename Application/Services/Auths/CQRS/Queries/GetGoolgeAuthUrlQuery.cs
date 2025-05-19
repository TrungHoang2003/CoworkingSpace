using Application.SharedServices;
using Domain.ResultPattern;
using MediatR;

namespace Application.AuthService.CQRS.Queries;

public sealed record GetGoogleAuthUrlQuery : IRequest<Result<string>>;

public class GetGoogleAuthUrlQueryHandler(GoogleAuthService googleAuthService)
    : IRequest<GetGoogleAuthUrlQuery>,
        IRequestHandler<GetGoogleAuthUrlQuery, Result<string>>
{
    public Task<Result<string>> Handle(GetGoogleAuthUrlQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var url = googleAuthService.GetGoogleAuthUrl();
            return Task.FromResult(Result<string>.Success(url));
        }
        catch (InvalidOperationException ex)
        {
            return Task.FromResult(Result<string>.Failure(
                new Error("Google.ClientIdNotFound", ex.Message)));
        }
    }
}