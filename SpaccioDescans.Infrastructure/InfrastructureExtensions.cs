using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        services.AddSqlPersistence(configuration.GetConnectionString("DefaultConnection"));
    }

    private static void AddSqlPersistence(this IServiceCollection services, string connectionString)
    {
        // factory?
        services.AddDbContext<SpaccioContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
            });
        });
    }
}