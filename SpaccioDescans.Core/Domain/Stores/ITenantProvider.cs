namespace SpaccioDescans.Core.Domain.Stores;

public interface ITenantProvider
{
    int GetUserTenant(string user);

    void SetUserTenant(string user, int tenantId);
}