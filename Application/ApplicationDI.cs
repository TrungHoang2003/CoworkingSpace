using System.Reflection;
using Application.BookingWindowService.CQRS.Commands;
using Application.BookingWindowService.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationDi
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cf => cf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(typeof(ApplicationDi).Assembly);
        
        return services;
    }
}