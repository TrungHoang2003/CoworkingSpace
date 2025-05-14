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

var env = builder.Environment.EnvironmentName;
Console.WriteLine($"Environment: {env}");

// Add services
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddApplication();

// Connection String config
string? connectionString;

if (builder.Environment.IsDevelopment())
{
    // Local development — dùng appsettings.json
    connectionString = builder.Configuration.GetConnectionString("MySqlConnectionStr");
    Console.WriteLine($"[DEV] Using connection string from appsettings.json: {connectionString}");
}
else
{
    // Production — lấy MYSQL_URL từ biến môi trường
    var mysqlUrl = Environment.GetEnvironmentVariable("MYSQL_URL");
    Console.WriteLine($"[PROD] MYSQL_URL from Environment: {mysqlUrl}");

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
        var database = uri.PathAndQuery.TrimStart('/');

        connectionString = $"Server={host};Port={port};Database={database};User Id={user};Password={password};";
    }
    else
    {
        connectionString = mysqlUrl; // fallback nếu là ADO.NET string
    }

    Console.WriteLine($"[PROD] Final connection string: {connectionString}");
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("No valid connection string found.");
}

// Add Infrastructure service
builder.Services.AddInfrastructure(connectionString);

// OpenAPI
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<BearerSercuritySchemeTransformer>();
});

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var secret = builder.Configuration["JWT:Secret"];
    var issuer = builder.Configuration["JWT:ValidIssuer"];
    var audience = builder.Configuration["JWT:ValidAudience"];

    if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
    {
        throw new Exception("JWT config is missing in appsettings.json or environment variables.");
    }

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

// CORS
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Redis
var redisOptions = new ConfigurationOptions
{
    EndPoints = { "localhost:6379" },
    AbortOnConnectFail = false,
};
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisOptions));

var app = builder.Build();

// Apply database migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    int maxRetries = 10;
    int delaySeconds = 2;

    for (int retry = 1; retry <= maxRetries; retry++)
    {
        try
        {
            Console.WriteLine($"Attempting migration (Attempt {retry}/{maxRetries})...");
            dbContext.Database.Migrate();
            Console.WriteLine("Migration completed successfully.");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration failed: {ex.Message}");
            if (retry == maxRetries)
            {
                Console.WriteLine("Max retries reached. Migration failed.");
                throw;
            }
            Console.WriteLine($"Retrying in {delaySeconds} seconds...");
            Thread.Sleep(delaySeconds * 1000);
        }
    }
}

// HTTP Request pipeline
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
// app.UseMiddleware<TokenValidateMiddleware>();
app.MapControllers();

// App URL Config
if (!app.Environment.IsDevelopment())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    app.Urls.Add($"http://0.0.0.0:{port}");
}

app.Run();
