using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entites;

namespace Domain.Entities;

public class SpaceCollection
{
   [Key] public int SpaceCollectionId { get; set; }
   
   public int SpaceId { get; set; }
   [ForeignKey("SpaceId")] Space? Space { get; set; }
   
   public int CollectionId { get; set; }
   [ForeignKey("CollectionId")] Collection? Collection { get; set; }
}