using Application.ExceptionService.CQRS.Commands;
using Domain.Entities;

namespace Application.ExceptionService.Mappings;

public static class ExceptionMappingExtensions
{
    public static ExceptionRule ToExceptionRule(this AddExceptionCommand command)
    {
        return new ExceptionRule
        {
            Unit = command.Unit,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            StartTime = command.StartTime,
            EndTime = command.EndTime,
            Description = command.Description,
            ExceptionRuleDays  = command.Days?.Select(day => new ExceptionRuleDay
            {
                DayOfWeek = day,
            }).ToList(),
        };
    }

    public static ExceptionRule ToExceptionRule(this UpdateExceptionCommand command, ExceptionRule existingRule)
    {
        existingRule.Unit = command.Unit;
        existingRule.StartDate = command.StartDate;
        existingRule.EndDate = command.EndDate;
        existingRule.StartTime = command.StartTime;
        existingRule.EndTime = command.EndTime;
        existingRule.Description = command.Description;
        existingRule.ExceptionRuleDays = command.Days?.Select(day => new ExceptionRuleDay
        {
            DayOfWeek = day,
        }).ToList();

        return existingRule;
    }
}
