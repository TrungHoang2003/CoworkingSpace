using System.Text;
using Application;
using CoworkingSpace.Middlewares;
using CoworkingSpace.Transformer;
using dotenv.net;
using Infrastructure;
using Infrastructure.DbHelper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddApplication();

// Lấy MYSQL_URL từ biến môi trường
var mysqlUrl = Environment.GetEnvironmentVariable("MYSQL_URL");
Console.WriteLine($"MYSQL_URL from Environment: {mysqlUrl}");

string? connectionString;
if (!string.IsNullOrEmpty(mysqlUrl))
{
    // Chuyển đổi từ URI-style sang ADO.NET
    if (mysqlUrl.StartsWith("mysql://"))
    {
        var uri = new Uri(mysqlUrl);
        var host = uri.Host;
        var portt = uri.Port;
        var user = uri.UserInfo.Split(':')[0];
        var password = uri.UserInfo.Split(':')[1];
        var database = uri.PathAndQuery.TrimStart('/');

        connectionString = $"Server={host};Port={portt};Database={database};User Id={user};Password={password};";
    }
    else
    {
        connectionString = mysqlUrl; // Sử dụng trực tiếp nếu đã ở định dạng ADO.NET
    }
}
else
{
    Console.WriteLine("Falling back to Configuration (e.g., appsettings.json).");
    connectionString = builder.Configuration.GetConnectionString("MySqlConnectionStr");
    Console.WriteLine($"MySqlConnectionStr from Configuration: {connectionString}");
}

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("No valid connection string found.");
    throw new Exception("Chuoi ket noi chua duoc thiet lap");
}

Console.WriteLine($"Using connection string: {connectionString}");

try
{
    builder.Services.AddInfrastructure(connectionString);
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to configure infrastructure services: {ex.Message}");
    throw;
}

builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSercuritySchemeTransformer>();});

builder.Services.AddAuthentication(options =>
    {
        // Chỉ định scheme mặc định cho Authenticate và Challenge
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var secret = builder.Configuration["JwtSettings:Secret"];
        var issuer = builder.Configuration["JwtSettings:Issuer"];
        var audience = builder.Configuration["JwtSettings:Audience"];

        if (string.IsNullOrEmpty(secret))
            throw new Exception("JWT Secret is not configured in environment variables.");
        if (string.IsNullOrEmpty(issuer))
            throw new Exception("JWT Issuer is not configured in environment variables.");
        if (string.IsNullOrEmpty(audience))
            throw new Exception("JWT Audience is not configured in environment variables.");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
    });

builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var options = new ConfigurationOptions
{
    EndPoints = {"localhost:6379"},
    AbortOnConnectFail = false,
};
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options));

var app = builder.Build();

// // Chạy migration với retry logic
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     int maxRetries = 10;
//     int delaySeconds = 2;
//
//     for (int retry = 1; retry <= maxRetries; retry++)
//     {
//         try
//         {
//             Console.WriteLine($"Attempting migration (Attempt {retry}/{maxRetries})...");
//             dbContext.Database.Migrate();
//             Console.WriteLine("Migration completed successfully.");
//             break; // Thoát vòng lặp nếu thành công
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Migration failed: {ex.Message}");
//             if (retry == maxRetries)
//             {
//                 Console.WriteLine("Max retries reached. Migration failed.");
//                 throw; // Ném lỗi sau khi hết số lần retry
//             }
//             Console.WriteLine($"Retrying in {delaySeconds} seconds...");
//             Thread.Sleep(delaySeconds * 1000); // Chờ trước khi thử lại
//         }
//     }
// }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Coworking Space API");
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        options.WithTheme(ScalarTheme.DeepSpace);
    });
}

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlerMiddleWare>();
//app.UseMiddleware<TokenValidateMiddleware>();
app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}"); // Đảm bảo lắng nghe trên 0.0.0.0

app.Run();
