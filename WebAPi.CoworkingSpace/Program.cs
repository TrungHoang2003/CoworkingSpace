using System.Text;
using Application;
using CoworkingSpace.Middlewares;
using CoworkingSpace.Transformer;
using dotenv.net;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

var connectionString = Environment.GetEnvironmentVariable("MYSQL_URL");
if (string.IsNullOrEmpty(connectionString))
{
    // Nếu không có, đọc từ file appsettings.json
    connectionString = builder.Configuration.GetConnectionString("MySqlConnectionStr");
}

builder.Services.AddInfrastructure(connectionString);

builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSercuritySchemeTransformer>();});

builder.Services.AddAuthentication(options =>
    {
        // Chỉ định scheme mặc định cho Authenticate và Challenge
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // TokenValidationParameters ...
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false,
            ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
            ValidAudience = builder.Configuration["Jwt:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
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
app.Run();
