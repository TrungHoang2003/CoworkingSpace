using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs;

public class ExceptionDto
{
    [Required] public ExceptionUnit Unit { get; set; }
    public int VenueId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? Description { get; set; }
    public bool ApplyAll{ get; set; } = true;
    public List<DayOfWeek?> Days { get; set; } 
    
   public List<int>? SpaceIds { get; set; } = [];

   public void Validate()
   {
       switch (Unit)
       {
           case ExceptionUnit.DateRange:
           {
               if (StartDate is null || EndDate is null)
               {
                   throw new Exception("StartDate and EndDate are required for DateRange unit.");
               }

               if (EndDate < StartDate)
               {
                   throw new Exception("EndDate must be greater than StartDate.");
               }
           }
               break;
           case ExceptionUnit.DaysOfWeek:
           {
               if (Days is null || Days.Count == 0)
               {
                   throw new Exception("Days are required for DaysOfWeek unit.");
               }
           }
               break;
           default:
               throw new ArgumentOutOfRangeException();
       }

       if (ApplyAll) return;
       if(SpaceIds is null || SpaceIds.Count == 0)
           throw new Exception("At least 1 space is required when ApplyAll is false.");
   }
}

public class AddExceptionRequest : ExceptionDto
{
    
}

public class UpdateExceptionRequest : ExceptionDto
{
    
}