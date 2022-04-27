using Blazored.LocalStorage;
using SpaccioDescans.Infrastructure.Persistence;

namespace SpaccioDescans.Web.Services;

public class StoreSelector : IStoreSelector
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILocalStorageService localStorageService;
    private readonly ITenantService tenantService;

    public StoreSelector(ILocalStorageService localStorageService, IHttpContextAccessor httpContextAccessor, ITenantService tenantService)
    {
        this.localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.tenantService = tenantService ?? throw new ArgumentNullException(nameof(tenantService));
    }

    private string? User => this.httpContextAccessor.HttpContext?.User.Identity?.Name;

    public async Task SetAsync(int store)
    {
        await this.localStorageService.SetItemAsync(this.User, store);
        this.tenantService.SetTenant(store);
    }

    public async Task<int> RetrieveAsync()
    {
        var store = await this.localStorageService.GetItemAsStringAsync(this.User).ConfigureAwait(true);

        return Convert.ToInt32(store);
    }
}