namespace Infrastructure.Interfaces;

public interface IGenericRepository<T> where T : class
{
    public Task Create(T entity);
    public Task Update(T entity);
    public Task Delete(T entity);
} 