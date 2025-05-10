using Dapper;
using Domain.Entities;
using Google.Apis.Upload;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public interface ISpaceImageRepository : IGenericRepository<SpaceImage>
{
   Task<SpaceImage?> GetById(int Id);
}
public class SpaceImageRepository(ApplicationDbContext dbContext, IConfiguration configuration) :GenericRepository<SpaceImage>(dbContext), ISpaceImageRepository
{
   public async Task<SpaceImage?> GetById(int Id)
   {
      var cnn = new MySqlServer(configuration).OpenConnection();
      const string sql = $"SELECT * FROM SpaceImage WHERE ImageId = @{nameof(Id)}";
      var result = await cnn.QueryFirstOrDefaultAsync(sql, new { Id });
      return result;
   }
}