namespace SpaccioDescans.Infrastructure.Persistence;

public class ConfigurationTenantProvider : ITenantProvider
{
    private readonly TenantConfiguration configuration;

    public ConfigurationTenantProvider(TenantConfiguration configuration)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public int GetTenantId()
    {
        return this.configuration.Id;
    }
}