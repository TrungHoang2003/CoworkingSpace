using Domain.Entities;

namespace Domain.ViewModel;

public class BookingWindowListViewModel
{
    public int BookingWindowId { get; set; }
    public int MinNotice { get; set; }
    public int? MaxNoticeDays { get; set; }
    public string? Workspace { get; set; } 
    public BookingTimeUnit? Unit { get; set; }
    
    public string MinNoticeString => $"{MinNotice}{(Unit == BookingTimeUnit.Days ? " Ngày" : " Giờ")}"; 
    public string MaxNoticeDisplay => MaxNoticeDays.HasValue ? $"{MaxNoticeDays}{(Unit == BookingTimeUnit.Days ? " Ngày" : " Không có")}" : " Không có";
}