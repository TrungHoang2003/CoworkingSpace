using Domain.Entites;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User: IdentityUser<int> 
{
    public string? FullName { get; set;}  
    public string? AvatarUrl { get; set;}
    
    // Navigation properties
    public ICollection<Venue>? Venues { get; set; }
    public ICollection<Reservation>? Reservations { get; set; } 
    public ICollection<Payment>? Payments { get; set; } 
    public ICollection<Review>? Reviews { get; set; } 
    public ICollection<Collection>? Collections { get; set; } 
    public ICollection<PaymentInfo>? PaymentInfos { get; set; } 
}