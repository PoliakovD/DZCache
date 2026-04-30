using DZCache.Data;
using DZCache.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace DZCache;

public class ProductsService(DzDbContext context, HybridCache cache)
{
    public async Task<IEnumerable<Product>> GetProducts()
    {
        var cacheKey = "products";
        
        var products = await cache.GetOrCreateAsync(
            cacheKey,
            async cancel => await context.Products.ToListAsync(cancel)
        );
        
        return products;
    }
    public async Task<Product> GetProduct(Guid id)
    {
        var cacheKey = $"product_{id}";
        
        var product = await cache.GetOrCreateAsync(
            cacheKey,
            async cancel => await context.Products.FirstAsync(p => p.Id == id, cancel)
        );
        
        return product;
    }
}