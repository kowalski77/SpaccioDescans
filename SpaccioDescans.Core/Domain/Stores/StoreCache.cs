namespace SpaccioDescans.Core.Domain.Stores;

public class StoreCache : IStoreCache
{
    private readonly Dictionary<string, int> tenants = new();
    private const int DefaultStore = 1;

    public int GetUserStore(string user)
    {
        if (string.IsNullOrWhiteSpace(user))
        {
            throw new ArgumentException($"'{nameof(user)}' cannot be null or whitespace.", nameof(user));
        }

        return tenants.TryGetValue(user, out var tenant) ?
            tenant : DefaultStore;
    }

    public void SetUserStore(string user, int tenantId)
    {
        if (string.IsNullOrWhiteSpace(user))
        {
            throw new ArgumentException($"'{nameof(user)}' cannot be null or whitespace.", nameof(user));
        }

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
