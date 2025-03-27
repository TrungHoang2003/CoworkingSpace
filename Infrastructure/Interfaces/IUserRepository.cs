using Domain.Entites;
using Infrastructure.Common;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interfaces;

public interface IUserRepository: IGenericRepository<User>
{
    Task<Result> UpdateAvatar(IFormFile file);
    Result<int> GetUserIdFromJwt();
}