using Domain.ResultPattern;
using FluentValidation;
using MediatR;

namespace Application.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result // Ép kiểu TResponse là Result hoặc kế thừa Result
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var errorMessages = string.Join("; ", failures.Select(f => f.ErrorMessage));
                // Tạo Result.Failure và ép kiểu về TResponse
                var failureResult = Result.Failure(new Error("Validation Errors", errorMessages));
                return (TResponse)failureResult;
            }
        }

        // Nếu pass validate thì gọi handler tiếp
        return await next();
    }
}