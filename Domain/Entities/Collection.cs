using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entites;
using Domain.Entities.WorkingSpace;

namespace Domain.Entities;

public class Collection
{
   [Key] public int CollectionId { get; set; } 
   
   public int UserId { get; set; }
   [ForeignKey("UserId")] public User? User { get; set; }
   
   public string? Name {get; set;}
   public string? Description { get; set; }
   
   public ICollection<SpaceCollection>? Spaces { get; set; } // Bộ sưu tập có thể chứa 1 hoặc nhiều không gian
}