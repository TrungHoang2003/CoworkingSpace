using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MySqlConnector;

namespace Infrastructure.DbHelper;

public abstract class DbConnection<T>
{
    public abstract T OpenConnection();
}

public class MySqlServer(IConfiguration configuration, IHostEnvironment env) : DbConnection<MySqlConnection>
{
    public override MySqlConnection OpenConnection()
    {
        string connectionString;

        if (env.IsDevelopment())
        {
            connectionString = configuration.GetConnectionString("MySqlConnectionStr") ?? throw new Exception("Connection string is not set in environment variables.");
        }
        else
        {
            var mysqlUrl = Environment.GetEnvironmentVariable("MYSQL_URL");

            if (string.IsNullOrEmpty(mysqlUrl))
            {
                throw new Exception("MYSQL_URL is not set in environment variables.");
            }

            if (mysqlUrl.StartsWith("mysql://"))
            {
                var uri = new Uri(mysqlUrl);
                var host = uri.Host;
                var port = uri.Port;
                var user = uri.UserInfo.Split(':')[0];
                var password = uri.UserInfo.Split(':')[1];
                var database = uri.AbsolutePath.TrimStart('/');

                connectionString = $"Server={host};Port={port};Database={database};User Id={user};Password={password};";
            }
            else
            {
                connectionString = mysqlUrl; // fallback nếu set sẵn dạng ADO.NET string
            }
        }

        var cnn = new MySqlConnection(connectionString);

        if (cnn.State == ConnectionState.Closed)
            cnn.Open();

        return cnn;
    }
}
