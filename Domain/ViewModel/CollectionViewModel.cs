using Domain.Entities;

namespace Domain.ViewModel;

public class CollectionViewModel
{
    public int CollectionId { get; set; }
    public string Name { get; set; }
    public int NumberOfSpaces { get; set; }
    public bool IsAdded { get; set; }
}