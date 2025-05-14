using System.Collections.Immutable;
using System.Reflection;
using Domain.Entites;
using Domain.Entities;
using Infrastructure.DbHelper;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using StackExchange.Redis;
using Role = Domain.Entities.Role;

namespace Infrastructure;

public static class InfrastructureDi
{
    public static void AddInfrastructure(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Connection string is not set in environment variables.");
            throw new Exception("Chuoi ket noi chua duoc thiet lap");
        }

        Console.WriteLine($"Using connection string: {connectionString}");

        try
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to configure ApplicationDbContext: {ex.Message}");
            throw;
        }
        
        services.AddIdentity<User, Role>(options =>
        {
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        }).AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddSingleton<JwtService>();
        services.AddSingleton<RedisService>();
        services.AddSingleton<CloudinaryService>();
        services.AddSingleton<GoogleAuthService>();
        services.AddSingleton<HttpClient>();
        services.AddScoped<DbConnection<MySqlConnection>, MySqlServer>();
        services.AddScoped<ISpaceRepository, SpaceRepository>();
        services.AddScoped<IAmenityRepository, AmenityRepository>();
        services.AddScoped<IVenueHolidayRepository, VenueHolidayRepository>();
        services.AddScoped<IVenueImageRepository, VenueImageRepository>();
        services.AddScoped<ISpaceImageRepository, SpaceImageRepository>();
        services.AddScoped<ISpaceAmenityRepository, SpaceAmenityRepository>();
        services.AddScoped<ISpaceTypeRepository, SpaceTypeRepository>();
        services.AddScoped<IExceptionRepository, ExceptionRepository>();
        services.AddScoped<IHolidayRepository, HolidayRepository>();
        services.AddScoped<IBookingWindowRepository, BookingWindowRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVenueAddressRepository, VenueAddressRepository>();
        services.AddScoped<IVenueTypeRepository, VenueTypeRepository>();
        services.AddScoped<IVenueRepository, VenueRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGuestHourRepository, GuestHourRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
    }
}