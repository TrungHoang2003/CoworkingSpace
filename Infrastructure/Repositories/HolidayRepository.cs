using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface IHolidayRepository : IGenericRepository<Venue>
{
    Task<List<Holiday>> GetAllHolidays();
    Task<Holiday?> GetById(int holidayId);
}

public class HolidayRepository(ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection) : GenericRepository<Venue>(dbContext), IHolidayRepository
{
    public async Task<List<Holiday>> GetAllHolidays()
    {
        var cnn = dbConnection.OpenConnection();
        
        const string sql = $"select * from Holiday";
        var result = await cnn.QueryAsync<Holiday>(sql);
        
        return result.ToList();
    }

    public async Task<Holiday?> GetById(int holidayId)
    {
        var cnn = dbConnection.OpenConnection();
        
        var sql = $"Select * from Holiday where HolidayId = {holidayId}";
        
        var result = await cnn.QueryFirstOrDefaultAsync<Holiday>(sql, new {Id = holidayId});
        return result;
    }
}