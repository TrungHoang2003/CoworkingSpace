using Microsoft.AspNetCore.Identity;

namespace Domain.Entites;

public class User: IdentityUser<int> 
{
    public string? FullName { get; set;}  
    public string? AvatarUrl { get; set;}
    
    // Navigation properties
    public ICollection<Space>? WorkingSpaces { get; set; } // One-to-Many with WorkingSpaces
    public ICollection<Reservation>? Reservations { get; set; } // One-to-Many with Reservations
    public ICollection<Payment>? Payments { get; set; } // One-to-Many with Payments
    public ICollection<Review>? Reviews { get; set; } // One-to-Many with Reviews
    public ICollection<Collection>? Collections { get; set; } // One-to-Many with Collections
}