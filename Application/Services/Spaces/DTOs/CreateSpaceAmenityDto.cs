namespace Application.Services.Spaces.DTOs;

public class CreateSpaceAmenityDto
{
    List<int>? AmenityIds { get; set; }
    public bool? Availability{ get; set; }
    public bool? HasFee{ get; set; }
}