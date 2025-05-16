using System.Runtime.CompilerServices;
using System.Security.Claims;
using Domain.Entites;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.DbHelper;
using Infrastructure.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public interface IUserRepository: IGenericRepository<User>
{
    Task<User?> GetById(int id);
    Task<User?> FindByNameAsync(string userName);
    Task<IList<string>> GetRolesAsync(User user);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<IdentityResult> CreateAsync(User user, string? password = null);
    Task<User?> FindByEmailAsync(string email);
    Task<IdentityResult> AddToRoleAsync(User user, string role);
}

public class UserRepository(UserManager<User> userManager, ApplicationDbContext dbContext) : GenericRepository<User>(dbContext), IUserRepository
{
    public async Task<User?> GetById(int id)
    {
        return await userManager.FindByIdAsync(id.ToString());
    }

    public async Task<User?> FindByNameAsync(string userName)
    {
        return await userManager.FindByNameAsync(userName);
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityResult> CreateAsync(User user, string? password = null)
    {
        return password == null
            ? await userManager.CreateAsync(user)
            : await userManager.CreateAsync(user, password);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetRolesAsync(User user)
    {
        return await userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> AddToRoleAsync(User user, string role)
    {
        return await userManager.AddToRoleAsync(user, role);
    }
}