using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface IPriceRepository : IGenericRepository<Price>
{
    Task<Price?> GetById(int priceId);
}

public class PriceRepository(ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection) : GenericRepository<Price>(dbContext), IPriceRepository
{
    public async Task<Price?> GetById(int priceId)
    {
        var cnn = dbConnection.OpenConnection();
        const string sql = "Select * from Price where Id = @priceId";
        var result = await cnn.QueryFirstOrDefaultAsync<Price>(sql, new { priceId });
        return result;
    }
}