namespace Infrastructure.Responses;

public class GoogleTokenResponse
{
   public string accessToken { get; set; }
   public string idToken { get; set; }
   public string refreshToken { get; set; }
}