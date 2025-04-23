using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public interface IHolidayRepository : IGenericRepository<Venue>
{
    Task<List<Holiday>> GetAllHolidays();
    Task<Holiday?> GetById(int holidayId);
}

public class HolidayRepository(ApplicationDbContext dbContext, IConfiguration configuration) : GenericRepository<Venue>(dbContext), IHolidayRepository
{
    public async Task<List<Holiday>> GetAllHolidays()
    {
        var cnn = new MySqlServer(configuration).OpenConnection();
        
        const string sql = $"select * from Holiday";
        var result = await cnn.QueryAsync<Holiday>(sql);
        
        return result.ToList();
    }

    public async Task<Holiday?> GetById(int holidayId)
    {
        var cnn = new MySqlServer(configuration).OpenConnection();
        
        var sql = $"Select * from Holiday where HolidayId = {holidayId}";
        
        var result = await cnn.QueryFirstOrDefaultAsync<Holiday>(sql, new {Id = holidayId});
        return result;
    }
}