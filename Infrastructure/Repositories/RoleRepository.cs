using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public interface IRoleRepository
{
    Task<bool> RoleExistsAsync(string roleName);
    Task<IdentityResult> CreateAsync(Role role);
}

public class RoleRepository(RoleManager<Role> roleManager): IRoleRepository
{
    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await roleManager.RoleExistsAsync(roleName);
    }

    public async Task<IdentityResult> CreateAsync(Role role)
    {
        return await roleManager.CreateAsync(role);
    }
}