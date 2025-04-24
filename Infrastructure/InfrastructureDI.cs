using System.Collections.Immutable;
using System.Reflection;
using Domain.Entites;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.DbHelper;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Role = Domain.Entities.Role;

namespace Infrastructure;

public static class InfrastructureDi
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddIdentity<User, Role>(options =>
        {
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        }).AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
           options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)); 
        });
        
        services.AddSingleton<JwtService>();
        services.AddSingleton<RedisService>();
        services.AddSingleton<CloudinaryService>();
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<ISpaceRepository, SpaceRepository>();
        services.AddScoped<IVenueHolidayRepository, VenueHolidayRepository>();
        services.AddScoped<IHolidayRepository, HolidayRepository>();
        services.AddScoped<IBookingWindowRepository, BookingWindowRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVenueAddressRepository, VenueAddressRepository>();
        services.AddScoped<IVenueTypeRepository, VenueTypeRepository>();
        services.AddScoped<IVenueRepository, VenueRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGuestHourRepository, GuestHourRepository>();
    }
}