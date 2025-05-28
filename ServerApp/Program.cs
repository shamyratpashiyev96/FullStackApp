
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseCors(x => x.
    AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.MapGet("/api/products", (HttpContext httpContext) =>
{
    var memoryCache = httpContext.RequestServices.GetRequiredService<IMemoryCache>();
    var cacheKey = "productList";
    var productList = new[]
    {
        new
        {
            Id = 1, Name = "Laptop", Price = 1200.50, Stock = 25, Category = new { Id = 101, Name = "Electronics" }
        },

        new
        {
            Id = 2, Name = "Headphones", Price = 50.00, Stock = 100,
            Category = new { Id = 102, Name = "Accessories" }
        }
    };

    if (memoryCache.TryGetValue(cacheKey, out var cacheResult))
    {
        return cacheResult;
    }
    else
    {
        memoryCache.Set(cacheKey, productList);
        return productList;
    }
});

app.Run();