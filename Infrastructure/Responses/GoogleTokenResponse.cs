using Newtonsoft.Json;

namespace Infrastructure.Responses;

public class GoogleTokenResponse
{
   public string access_token { get; set; }
   
   public string id_token { get; set; }
}