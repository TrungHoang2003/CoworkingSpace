using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Entites;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Common;

public class JwtService(IConfiguration configuration)
{
    public static int getUserIdFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
           
            if(!handler.CanReadToken(token))
                throw new Exception("Cant read token or invalid token");
           
            var jwtToken = handler.ReadJwtToken(token);
           
            var userIdStr = jwtToken.Claims.First(c=>c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            
            if (string.IsNullOrEmpty(userIdStr))
            {
                throw new Exception("Cant find userId");
            }
            
            var userId = int.TryParse(userIdStr, out var id) ?id: throw new Exception("UserId is not a number");
            
            return userId;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi lấy userId từ token: {ex.Message}");
            throw;
        }
    }

    public static string getRoleFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
           
            if(!handler.CanReadToken(token))
                throw new Exception("Invalid token");
           
            var jwtToken = handler.ReadJwtToken(token);
           
            var role = jwtToken.Claims.First(c=>c.Type == ClaimTypes.Role)?.Value;
            
            if (string.IsNullOrEmpty(role))
            {
                throw new Exception("Cant find roleName");
            }

            return role;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi lấy userId từ token: {ex.Message}");
            throw;
        } 
    }
    
    public string? GenerateJwtToken(User user, string? role)
    {
        var accesstokenValidity = getAccessTokenValidity();
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new []
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), 
            new Claim(JwtRegisteredClaimNames.Email, user.Email??"Email:null"),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName??"Username:null"),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, role ?? "Role:Not set")
        };

        var accessToken = new JwtSecurityToken(
            expires: DateTime.Now.AddMinutes(accesstokenValidity),
            claims: claims,
            signingCredentials: creds);
        
        return new JwtSecurityTokenHandler().WriteToken(accessToken); 
    }
    
    public string? GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var base64String = Convert.ToBase64String(randomNumber);
        return base64String.TrimEnd('=');
    }

    public int getAccessTokenValidity()
    {
        _ = int.TryParse(configuration["JWT:AccessTokenValidityInMinutes"], out var accessTokenValidity);
        
        return accessTokenValidity;
    }
    
    public int getRefreshTokenValidity()
    {
        _ = int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out var refreshTokenValidity);
        
        return refreshTokenValidity;
    }

}
