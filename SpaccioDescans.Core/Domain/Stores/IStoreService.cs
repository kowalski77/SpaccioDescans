namespace SpaccioDescans.Core.Domain.Stores;

public interface IStoreService
{
    Task<Store> GetStoreByUserAsync(string userName);
}