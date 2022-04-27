namespace SpaccioDescans.Web.Services;

public interface IStoreSelector
{
    Task SetAsync(int store);

    Task<int> RetrieveAsync();
}