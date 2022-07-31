namespace SpaccioDescans.Core.Domain.Stores;

public class StoreCache : IStoreCache
{
    private readonly Dictionary<string, int> tenants = new();

    public int GetUserStore(string user)
    {
        return tenants.TryGetValue(user, out var tenant) ? 
            tenant : 
            throw new InvalidOperationException($"No tenant found for user: {user}");
    }

    public void SetUserStore(string user, int tenantId)
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
