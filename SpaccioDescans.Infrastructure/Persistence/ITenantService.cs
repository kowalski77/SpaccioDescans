namespace SpaccioDescans.Infrastructure.Persistence;

public interface ITenantService
{
    int Tenant { get; }

    void SetTenant(int tenant);
}