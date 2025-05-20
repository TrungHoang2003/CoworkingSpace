using System.Reflection;
using Application.BookingWindowService.CQRS.Commands;
using Application.BookingWindowService.Validators;
using Application.PipelineBehaviors;
using Application.SharedServices;
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

        services.AddSingleton<VnPayService>(); 
        services.AddSingleton<JwtService>();
        services.AddSingleton<RedisService>();
        services.AddSingleton<CloudinaryService>();
        services.AddSingleton<GoogleAuthService>();
        return services;
    }
}