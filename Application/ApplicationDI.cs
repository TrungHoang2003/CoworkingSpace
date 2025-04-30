using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationDi
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cf => cf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}