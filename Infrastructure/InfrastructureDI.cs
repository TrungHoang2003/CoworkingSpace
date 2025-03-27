using System.Reflection;
using Domain.Entites;
using Infrastructure.Common;
using Infrastructure.DbHelper;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure;

public static class InfrastructureDI
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        }).AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
           options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)); 
        });

        services.AddSingleton<IConnectionMultiplexer>(
            sp => ConnectionMultiplexer.Connect("localhost:6379"));
        
        services.AddSingleton<JwtService>();
        services.AddSingleton<RedisService>();
        services.AddSingleton<CloudinaryService>();
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVenueRepository, VenueRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}