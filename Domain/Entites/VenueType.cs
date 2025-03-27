using System.ComponentModel.DataAnnotations;

namespace Domain.Entites;

public class VenueType
{
   [Key] public int VenueTypeId { get; set; }
   
   public string? Name { get; set; }
   public string? Description { get; set; }
}