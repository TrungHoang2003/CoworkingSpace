using Dapper;
using Domain.Entites;
using Infrastructure.Common;
using Infrastructure.DbHelper;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class WorkingSpaceRepository(ApplicationDbContext dbContext, IConfiguration configuration) : GenericRepository<Space>(dbContext), IWorkingSpaceRepository
{
    public async Task<Result<IEnumerable<Space>>> GetAllWorkingSpacesAsync()
    {
        var connection = new MySqlServer(configuration).OpenConnection();

        try
        {
            var sql = $"select * from Spaces";
            var result = await connection.QueryAsync<Space>(sql);
            
           return Result<IEnumerable<Space>>.Success(result); 
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting all working spaces", e);
        }
    }
    
}