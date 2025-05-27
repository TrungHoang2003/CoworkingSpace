using System.Collections.ObjectModel;
using Dapper;
using Domain.Entities;
using Domain.ViewModel;
using Infrastructure.DbHelper;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface ICollectionRepository: IGenericRepository<Collection>
{
    Task<List<CollectionViewModel>> GetUserSpaceCollectionList(int userId, int spaceId);
    Task<Collection?> GetById(int id);
}

public class CollectionRepository(ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection) : GenericRepository<Collection>(dbContext), ICollectionRepository
{
    public async Task<Collection?> GetById(int id)
    {
        var cnn = dbConnection.OpenConnection();
        var sql = $"select * from Collection where CollectionId = {id}";
        var result = await cnn.QueryFirstOrDefaultAsync<Collection>(sql, new { id });
        return result;
    }

    public async Task<List<CollectionViewModel>> GetUserSpaceCollectionList(int userId, int spaceId)
    {
        var cnn = dbConnection.OpenConnection();
        const string sql = """
                           SELECT 
                               c.CollectionId,
                               c.Name,
                               COUNT(sc.SpaceId) AS NumberOfSpaces,
                               CASE WHEN MAX(CASE WHEN sc.SpaceId = @spaceId THEN 1 ELSE 0 END) = 1 THEN 1 ELSE 0 END AS IsAdded
                           FROM Collection c
                           LEFT JOIN SpaceCollection sc ON c.CollectionId = sc.CollectionId
                           WHERE c.UserId = @userId
                           GROUP BY c.CollectionId, c.Name
                           ORDER BY c.Name
                           """;
        var result = await cnn.QueryAsync<CollectionViewModel>(sql, new{ spaceId, userId });
        return result.ToList();
    }
}