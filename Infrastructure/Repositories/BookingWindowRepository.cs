using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface IBookingWindowRepository : IGenericRepository<BookingWindow>
{
   Task<BookingWindow?> GetById(int bookingWindowId);
}
public class BookingWindowRepository(DbConnection<MySqlConnection> dbConnection, ApplicationDbContext dbContext) : GenericRepository<BookingWindow>(dbContext), IBookingWindowRepository
{
   public async Task<BookingWindow?> GetById(int bookingWindowId)
   {
      var cnn = dbConnection.OpenConnection();

      var sql = $"select * from BookingWindow where BookingWindowId = {bookingWindowId}";
      var result = await cnn.QueryFirstOrDefaultAsync<BookingWindow>(sql, new { BookingWindowId = bookingWindowId });
      
      return result;
   }
}