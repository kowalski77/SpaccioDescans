using Microsoft.AspNetCore.Identity;

namespace SpaccioDescans.Web.Areas.Identity.Support;

public static class IdentityExtensions
{
    public static async Task SeedUsersAsync(this WebApplication host)
    {
        ArgumentNullException.ThrowIfNull(host);

        if (!host.Environment.IsDevelopment())
        {
            return;
        }

        using var scope = host.Services.CreateAsyncScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        
        var identitySettings = host.Configuration.GetSection(nameof(IdentitySettings)).Get<IdentitySettings>();
        if(identitySettings.SpaccioUsers is null)
        {
            return;
        }

        foreach (var user in identitySettings.SpaccioUsers)
        {
            await CreateSpaccioUserAsync(userManager, user);
        }
    }

    private static async Task CreateSpaccioUserAsync(UserManager<IdentityUser> userManager, SpaccioUser user)
    {
        var identityUser = await userManager.FindByEmailAsync(user.Email).ConfigureAwait(false);
        if (identityUser is not null)
        {
            return;
        }

        identityUser = new IdentityUser
        {
            UserName = user.UserName,
            Email = user.Email,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(identityUser, user.Password).ConfigureAwait(false);
    }
}
