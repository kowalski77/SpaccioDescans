namespace SpaccioDescans.Core.Domain.Stores;

public interface IStoreCache
{
    int GetUserStore(string user);

    void SetUserStore(string user, int tenantId);
}