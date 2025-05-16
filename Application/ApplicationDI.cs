using System.Reflection;
using Application.Behaviors;
using Application.BookingWindowService.CQRS.Commands;
using Application.BookingWindowService.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationDi
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(ApplicationDi).Assembly
        ));        services.AddValidatorsFromAssembly(typeof(ApplicationDi).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        
        return services;
    }
}