using Dapper;
using Domain.Entities;
using Google.Apis.Upload;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface ISpaceAssetRepository : IGenericRepository<SpaceAsset>
{
   Task<SpaceAsset?> GetById(int id);
   Task<SpaceAsset?> GetByUrl(int spaceId, string url);
   Task<SpaceAsset?> GetByType(int spaceId, SpaceAssetType type);
}

public class SpaceAssetRepository(ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection)
   : GenericRepository<SpaceAsset>(dbContext), ISpaceAssetRepository
{
   public async Task<SpaceAsset?> GetById(int Id)
   {
      var cnn = dbConnection.OpenConnection();
      const string sql = $"SELECT * FROM SpaceAsset WHERE ImageId = @{nameof(Id)}";
      var result = await cnn.QueryFirstOrDefaultAsync(sql, new { Id });
      return result;
   }

   public async Task<SpaceAsset?> GetByUrl(int spaceId, string url)
   {
      var cnn = dbConnection.OpenConnection();
      const string sql = $"SELECT * FROM SpaceAsset WHERE Url = @{nameof(url)} and SpaceId = @{nameof(spaceId)}";
      var result = await cnn.QueryFirstOrDefaultAsync(sql, new { url, spaceId });
      return result;
   }

   public async Task<SpaceAsset?> GetByType(int spaceId, SpaceAssetType type)
   {
      var cnn = dbConnection.OpenConnection();
      const string sql = $"SELECT * FROM SpaceAsset WHERE Type = @{nameof(type)} and SpaceId = @{nameof(spaceId)}";
      var result = await cnn.QueryFirstOrDefaultAsync(sql, new { type, spaceId });
      return result;
   }
}