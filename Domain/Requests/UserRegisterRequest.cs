using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class UserRegisterRequest
{
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? FullName { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    
    public string? AvatarUrl { get; set; }
}