using Dapper;
using Domain.Entites;
using Infrastructure.Common;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public interface ISpaceRepository: IGenericRepository<Space>
{
   Task<Result<IEnumerable<Space>>> GetAllWorkingSpacesAsync();
   Task<Space?> GetById(int spaceId);
}
public class SpaceRepository(ApplicationDbContext dbContext, IConfiguration configuration) : GenericRepository<Space>(dbContext), ISpaceRepository
{
    public async Task<Result<IEnumerable<Space>>> GetAllWorkingSpacesAsync()
    {
        var connection = new MySqlServer(configuration).OpenConnection();

        try
        {
            var sql = $"select * from Spaces";
            var result = await connection.QueryAsync<Space>(sql);
            
           return Result<IEnumerable<Space>>.Success(result); 
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting all working spaces", e);
        }
    }

    public async Task<Space?> GetById(int spaceId)
    {
        var cnn = new MySqlServer(configuration).OpenConnection();
        
        var sql = "Select * from Spaces where SpaceId = @spaceId";
        var result = await cnn.QueryFirstOrDefaultAsync<Space>(sql, new { SpaceId = spaceId });

        return result;
    }
}