using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs;

public class UserRegisterDTO
{
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? FullName { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}