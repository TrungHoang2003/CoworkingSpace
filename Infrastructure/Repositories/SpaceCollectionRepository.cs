using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface ISpaceCollectionRepository: IGenericRepository<SpaceCollection>
{
   Task<SpaceCollection?> GetById(int spaceCollectionId, int spaceId);
}
public class SpaceCollectionRepository(ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection) :GenericRepository<SpaceCollection>(dbContext), ISpaceCollectionRepository
{
   public async Task<SpaceCollection?> GetById(int spaceCollectionId, int spaceId)
   {
       var cnn = dbConnection.OpenConnection();
      const string sql = "Select * from SpaceCollection where SpaceCollectionId = @spaceCollectionId and SpaceId = @spaceId";
      var result = await cnn.QueryFirstOrDefaultAsync<SpaceCollection>(sql, new { spaceCollectionId, spaceId });
      return result;
   }
}