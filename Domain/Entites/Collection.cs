using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites;

public class Collection
{
   [Key]
   public int CollectionId { get; set; } 
   
   public int UserId { get; set; }
   
   [ForeignKey("UserId")]
   public User? User { get; set; }
   
   public ICollection<Space>? WorkingSpaces { get; set; }
}