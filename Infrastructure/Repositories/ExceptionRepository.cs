using Domain.Entities;
using Infrastructure.DbHelper;

namespace Infrastructure.Repositories;

public interface IExceptionRepository: IGenericRepository<ExceptionRule>
{
    // Define methods for exception handling, logging, etc.
}

public class ExceptionRepository(ApplicationDbContext dbContext) : GenericRepository<ExceptionRule>(dbContext), IExceptionRepository
{
    
}