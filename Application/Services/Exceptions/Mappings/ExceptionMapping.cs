using Application.Services.Exceptions.CQRS.Commands;
using Domain.Entities;

namespace Application.Services.Exceptions.Mappings;

public static class ExceptionMapping
{
    public static ExceptionRule ToExceptionRule(this AddExceptionCommand command)
    {
        return new ExceptionRule
        {
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            StartTime = command.StartTime,
            EndTime = command.EndTime,
            Description = command.Description,
        };
    }

    public static ExceptionRule ToExceptionRule(this UpdateExceptionCommand command, ExceptionRule existingRule)
    {
        existingRule.StartDate = command.StartDate;
        existingRule.EndDate = command.EndDate;
        existingRule.StartTime = command.StartTime;
        existingRule.EndTime = command.EndTime;
        existingRule.Description = command.Description;
        return existingRule;
    }
}
