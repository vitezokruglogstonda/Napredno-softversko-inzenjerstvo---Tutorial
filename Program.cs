using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Tutorial.Middlewares;
using Tutorial.Models;
using Tutorial.Models.Database;
using Tutorial.Services.AccountService;
using Tutorial.Services.CacheService;
using Tutorial.Services.ItemService;

var builder = WebApplication.CreateBuilder(args);

//Add connections to storage
builder.Services.AddDbContext<CustomDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));

string? redisConnectionString = builder.Configuration.GetSection("Redis").GetSection("ConnectionString").Value;
ConnectionMultiplexer? multiplexer = ConnectionMultiplexer.Connect(redisConnectionString!);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

//Add services to the DI container
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IItemService, ItemService>();

//Bind classes to objects in appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

//Setup CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("JWT", "RefreshToken");
        });
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowOrigin");

app.UseMiddleware<AuthMiddleware>();

//app.UseAuthorization();

app.Run();