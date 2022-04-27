using SpaccioDescans.Infrastructure.Persistence;

namespace SpaccioDescans.Web.Services;

public class TenantService : ITenantService
{
    public int Tenant { get; private set; }

    public void SetTenant(int tenant)
    {
        this.Tenant = tenant;
    }
}