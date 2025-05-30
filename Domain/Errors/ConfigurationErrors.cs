using Domain.ResultPattern;

namespace Infrastructure.Errors;

public sealed record ConfigurationErrors
{
   public static readonly Error ClientIdNotFound = new ("Configuration Error", "ClientId not found"); 
   public static readonly Error ClientSecretNotFound = new ("Configuration Error", "ClientSecret not found"); 
}