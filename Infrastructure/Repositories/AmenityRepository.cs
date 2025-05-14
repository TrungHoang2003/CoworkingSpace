using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface IAmenityRepository : IGenericRepository<Amenity>
{
    Task<Amenity?> GetById(int amenityId);
    Task<bool> FindById(int amenityId);
}

public class AmenityRepository(DbConnection<MySqlConnection> dbConnection, ApplicationDbContext dbContext) : GenericRepository<Amenity>(dbContext), IAmenityRepository
{
    public async Task<Amenity?> GetById(int amenityId)
    {
        var cnn = dbConnection.OpenConnection();
        
        const string sql = "select * from Amenity where AmenityId = @amenityId";
        var result = await cnn.QueryFirstOrDefaultAsync<Amenity>(sql, new { AmenityId = amenityId });
        return result;
    }

    public async Task<bool> FindById(int amenityId)
    {
        var cnn = dbConnection.OpenConnection();
        
        const string sql = "select count(*) from Amenity where AmenityId = @amenityId";
        var result = await cnn.ExecuteScalarAsync<int>(sql, new { AmenityId = amenityId });
        return result > 0;
    }
}