using System.Text.Json;
using Domain.ResultPattern;
using Google.Apis.Auth;
using Infrastructure.Errors;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class GoogleAuthService(IConfiguration configuration, HttpClient httpClient)
{
    public string GetGoogleAuthUrl()
    {
        var clientId = configuration["Authentication:Google:ClientId"];
        if (string.IsNullOrEmpty(clientId))
            throw new InvalidOperationException("Google Client ID not found");

        var redirectUri = "http://localhost:5196/Authentication/GoogleCallback";
        var scope = "openid profile email";
        var state = Guid.NewGuid().ToString();

        return $"https://accounts.google.com/o/oauth2/v2/auth?" +
               $"client_id={clientId}&" +
               $"redirect_uri={redirectUri}&" +
               $"response_type=code&" +
               $"scope={scope}&" +
               $"access_type=offline&" +
               $"state={state}";
    }

    public async Task<Result<GoogleJsonWebSignature.Payload>> ValidateGoogleCodeAsync(string code)
    {
        var tokenUrl = "https://oauth2.googleapis.com/token";
        var clientId = configuration["Authentication:Google:ClientId"];
        var clientSecret = configuration["Authentication:Google:ClientSecret"];

        if (string.IsNullOrEmpty(clientId))
            return Result<GoogleJsonWebSignature.Payload>.Failure(ConfigurationErrors.ClientIdNotFound);
        if (string.IsNullOrEmpty(clientSecret))
            return Result<GoogleJsonWebSignature.Payload>.Failure(ConfigurationErrors.ClientSecretNotFound);

        var values = new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "redirect_uri", "http://localhost:5196/Authentication/GoogleCallback" },
            { "grant_type", "authorization_code" }
        };

        try
        {
            var content = new FormUrlEncodedContent(values);
            var response = await httpClient.PostAsync(tokenUrl, content);
            if (!response.IsSuccessStatusCode)
                return Result<GoogleJsonWebSignature.Payload>.Failure(
                    new Error("Google.AuthFailed", $"Failed to retrieve token: {response.StatusCode}"));

            var responseString = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<GoogleTokenResponse>(responseString);
            if (tokenData?.id_token == null)
                return Result<GoogleJsonWebSignature.Payload>.Failure(
                    new Error("Google.InvalidToken", "Invalid ID token"));

            var payload = await GoogleJsonWebSignature.ValidateAsync(tokenData.id_token);
            return Result<GoogleJsonWebSignature.Payload>.Success(payload);
        }
        catch (Exception ex)
        {
            return Result<GoogleJsonWebSignature.Payload>.Failure(
                new Error("Google.AuthException", $"Error validating Google code: {ex.Message}"));
        }
    }
}

public class GoogleTokenResponse
{
    public string access_token { get; set; } = string.Empty;
    public string id_token { get; set; } = string.Empty;
    public string refresh_token { get; set; } = string.Empty;
    public int expires_in { get; set; }
    public string scope { get; set; } = string.Empty;
}