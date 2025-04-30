using Dapper;
using Domain.Entites;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public interface IVenueAddressRepository: IGenericRepository<VenueAddress>
{
    void UpdateVenueFullAddress(VenueAddress venueAddress);
    
    Task<VenueAddress?> GetById(int venueAddressId);
}

public class VenueAddressRepository(ApplicationDbContext dbContext, IConfiguration configuration) : GenericRepository<VenueAddress>(dbContext), IVenueAddressRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public void UpdateVenueFullAddress(VenueAddress venueAddress)
    {
        venueAddress.FullAddress = $"{venueAddress.Street}, {venueAddress.District}, {venueAddress.City}";
        _dbContext.Update(venueAddress);
    }

    public Task<VenueAddress?> GetById(int venueAddressId)
    {
        var cnn = new MySqlServer(configuration).OpenConnection();

        try
        {
            const string sql = "select * from VenueAddress where VenueAddressId = @venueAddressId";
            var result = cnn.QueryFirstOrDefaultAsync<VenueAddress>(sql, new { VenueAddressId = venueAddressId });
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting venue address by id", e);
        }
    }
}