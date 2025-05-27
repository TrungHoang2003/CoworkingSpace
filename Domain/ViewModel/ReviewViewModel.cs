namespace Domain.ViewModel;

public class ReviewViewModel
{
    public string FullName{ get; set; } 
    public string? UserAvatarUrl { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}