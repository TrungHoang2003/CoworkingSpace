using Infrastructure.DbHelper;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GenericRepository<T>(ApplicationDbContext dbContext): IGenericRepository<T> where T : class
{
    public async Task Create(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
    }

    public Task Update(T entity)
    {
        dbContext.Set<T>().Update(entity);
        return Task.FromResult(entity);
    }

    public Task Delete(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        return Task.FromResult(entity);
    }
}