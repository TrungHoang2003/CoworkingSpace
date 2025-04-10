using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs;

public class UserLoginRequest
{
    [Required]
    public string? UserName { get; set; }
    [Required] public string? Password { get; set; }
}