using Dapper;
using Domain.Entities;
using Domain.ViewModel;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface IBookingWindowRepository : IGenericRepository<BookingWindow>
{
   Task<BookingWindow?> GetById(int bookingWindowId);
   Task<List<BookingWindowListViewModel>> GetByVenueId(int venueId);
}
public class BookingWindowRepository(DbConnection<MySqlConnection> dbConnection, ApplicationDbContext dbContext) : GenericRepository<BookingWindow>(dbContext), IBookingWindowRepository
{
   public async Task<BookingWindow?> GetById(int bookingWindowId)
   {
      var cnn = dbConnection.OpenConnection();

      var sql = $"select * from BookingWindow where BookingWindowId = @bookingWindowId";
      var result = await cnn.QueryFirstOrDefaultAsync<BookingWindow>(sql, new { BookingWindowId = bookingWindowId });
      
      return result;
   }

   public async Task<List<BookingWindowListViewModel>> GetByVenueId(int venueId)
   {
      var cnn = dbConnection.OpenConnection();

      const string sql = """
                         select bw.*, s.name AS Workspace from BookingWindow bw
                         left join Space s On s.BookingWindowId = bw.BookingWindowId
                         where VenueId = @venueId
                         """;
      var result = await cnn.QueryAsync<BookingWindowListViewModel>(sql, new { VenueId = venueId });
      return result.ToList();
   }
}