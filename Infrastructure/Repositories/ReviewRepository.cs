using Dapper;
using Domain.Entities;
using Domain.ViewModel;
using Infrastructure.DbHelper;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface IReviewRepository : IGenericRepository<Review>
{
    Task<List<ReviewViewModel>> GetSpaceReviews(int spaceId);
}
public class ReviewRepository(DbConnection<MySqlConnection> dbConnection, ApplicationDbContext dbContext) : GenericRepository<Review>(dbContext), IReviewRepository
{
    public async Task<List<ReviewViewModel>> GetSpaceReviews(int spaceId)
    {
        var cnn = dbConnection.OpenConnection();
        var sql = """
            SELECT r.*, u.FullName AS FullName, u.AvatarUrl
            FROM Review r
            JOIN AspNetUsers u ON r.CustomerId = u.Id
            WHERE r.SpaceId = @spaceId
            """;
        var result = await cnn.QueryAsync<ReviewViewModel>(sql, new { SpaceId = spaceId });
        return result.ToList();
    } 
}