using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.DbHelper;

public class MySqlServer(IConfiguration configuration): DbConnection<MySqlConnection>
{
    public override MySqlConnection OpenConnection()
    {
        var cnn = new MySqlConnection(configuration.GetConnectionString("MySqlConnectionStr"));
        
        if(cnn.State == ConnectionState.Closed)
            cnn.Open();
        return cnn;
    }
}