using DZCache.Data;
using DZCache.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace DZCache;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Настройка Redis для L2 кеша
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
            options.InstanceName = "DzCache";
        });

        // Настройка гибридного кеша
        builder.Services.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5), // Общее время жизни кеша
                LocalCacheExpiration = TimeSpan.FromMinutes(1) // Время жизни в L1
            };
        });
        builder.Services.AddDbContext<DzDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
                .LogTo(Console.WriteLine, LogLevel.Information));
        
        builder.Services.AddScoped<ProductsService>();
       
        var app = builder.Build();
        
        

        app.MapGet("/products", (ProductsService s) => s.GetProducts());
        app.MapGet("/product/{id}", (ProductsService s, Guid id) => s.GetProduct(id));

        await app.RunAsync();
    }
}