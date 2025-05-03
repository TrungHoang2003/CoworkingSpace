using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public interface IExceptionRepository: IGenericRepository<ExceptionRule>
{
    Task<ExceptionRule?> GetById(int id);
}

public class ExceptionRepository(ApplicationDbContext dbContext, IConfiguration configuration) : GenericRepository<ExceptionRule>(dbContext), IExceptionRepository
{
    public async Task<ExceptionRule?> GetById(int id)
    {
        var cnn = new MySqlServer(configuration).OpenConnection();
        
        var sql = $"Select * from ExcetionRule where ExceptionId = {id}";
        var result = await cnn.QueryFirstOrDefaultAsync<ExceptionRule>(sql);
        return result;
    }
}