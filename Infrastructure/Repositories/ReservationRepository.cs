using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface IReservationRepository: IGenericRepository<Reservation>
{
    Task SaveChangesAsync();
    Task<Reservation?> GetById(int reservationId);
}

public class ReservationRepository(ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection) : GenericRepository<Reservation>(dbContext), IReservationRepository
{
    public async Task<Reservation?> GetById(int reservationId)
    {
        var cnn = dbConnection.OpenConnection();
        const string sql = "Select * from Reservation where ReservationId = @reservationId";
        var result = await cnn.QueryFirstOrDefaultAsync<Reservation>(sql, new { ReservationId = reservationId });
        return result;
    }
    
    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}