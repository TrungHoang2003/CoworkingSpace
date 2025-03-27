using Infrastructure.DbHelper;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext, IVenueRepository venue, IUserRepository user) : IUnitOfWork
{
    public IVenueRepository Venue { get; } = venue;
    public IUserRepository User { get; } = user;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
       return await dbContext.SaveChangesAsync(cancellationToken);
    }
}