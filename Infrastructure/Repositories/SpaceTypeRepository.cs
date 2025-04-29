using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public interface ISpaceTypeRepository : IGenericRepository<SpaceType>
{
    Task<SpaceType?> GetById(int spaceTypeId);
    Task<bool> FindById(int spaceTypeId);
}

public class SpaceTypeRepository(IConfiguration configuration, ApplicationDbContext dbContext) : GenericRepository<SpaceType>(dbContext), ISpaceTypeRepository
{
    public async Task<SpaceType?> GetById(int spaceTypeId)
    {
        var cnn = new MySqlServer(configuration).OpenConnection();
        
        const string sql = "select * from SpaceType where SpaceTypeId = @spaceTypeId";
        var result = await cnn.QueryFirstOrDefaultAsync<SpaceType>(sql, new { SpaceTypeId = spaceTypeId });
        return result;
    }

    public async Task<bool> FindById(int spaceTypeId)
    {
        var cnn = new MySqlServer(configuration).OpenConnection();

        const string sql = "select count(*) from SpaceType where SpaceTypeId = @spaceTypeId";
        var result = await cnn.ExecuteScalarAsync<int>(sql, new { SpaceTypeId = spaceTypeId });
        return result > 0;
    }
}