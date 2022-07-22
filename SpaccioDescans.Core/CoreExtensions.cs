using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SpaccioDescans.Core;

public static class CoreExtensions
{
    public static void AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var querySettings = new QuerySettings
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection")
        };
        services.AddSingleton(querySettings);
    }
}