using Application.BookingWindowService.CQRS.Commands;
using Application.Services.BookingWindows.CQRS.Commands;
using Domain.Entities;

namespace Application.BookingWindowService.Mappings;

public static class BookingWindowMapping
{
    public static BookingWindow ToBookingWindow(this AddBookingWindowCommand command)
    {
        return new BookingWindow
        {
            MinNotice = command.MinNotice,
            MaxNoticeDays = command.MaxNoticeDays,
            Unit = command.Unit,
            DisplayOnCalendar = command.DisplayOnCalendar,
        };
    }

    public static BookingWindow ToBookingWindow(this UpdateBookingWindowCommand command, BookingWindow existingWindow)
    {
        existingWindow.MinNotice = command.MinNotice;
        existingWindow.MaxNoticeDays = command.MaxNoticeDays;
        existingWindow.Unit = command.Unit;
        existingWindow.DisplayOnCalendar = command.DisplayOnCalendar;

        return existingWindow;
    }
}
