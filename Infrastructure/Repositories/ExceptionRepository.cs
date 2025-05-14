using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface IExceptionRepository: IGenericRepository<ExceptionRule>
{
    Task<ExceptionRule?> GetById(int id);
}

public class ExceptionRepository(ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection) : GenericRepository<ExceptionRule>(dbContext), IExceptionRepository
{
    public async Task<ExceptionRule?> GetById(int id)
    {
        var cnn = dbConnection.OpenConnection();
        
        var sql = $"Select * from ExceptionRule where ExceptionId = {id}";
        var result = await cnn.QueryFirstOrDefaultAsync<ExceptionRule>(sql);
        return result;
    }
}