using Microsoft.AspNetCore.Http;

namespace SpaccioDescans.Infrastructure.Persistence;

public class HttpContextTenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public HttpContextTenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public int GetTenantId()
    {
        var host = this.httpContextAccessor.HttpContext?.Request.Host.Host;
        var tenantId = host switch
        {
            var x when x!.Contains("terrassa",StringComparison.OrdinalIgnoreCase) => 1,
            var x when x!.Contains("matadepera", StringComparison.OrdinalIgnoreCase) => 2,
            _ => throw new ArgumentOutOfRangeException()
        };

        return tenantId;
    }
}