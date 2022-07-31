namespace SpaccioDescans.Core.Domain.Stores;

public class TenantProvider : ITenantProvider
{
    private readonly Dictionary<string, int> tenants = new();

    public int GetUserTenant(string user)
    {
        return tenants.TryGetValue(user, out var tenant) ? 
            tenant : 
            throw new InvalidOperationException($"No tenant found for user: {user}");
    }

    public void SetUserTenant(string user, int tenantId)
    {
        if (this.tenants.ContainsKey(user))
        {
            this.tenants[user] = tenantId;
        }
        else
        {
            this.tenants.Add(user, tenantId);
        }
    }
}
