namespace SpaccioDescans.Web.Areas.Identity.Support;

public class IdentitySettings
{    
    public IReadOnlyCollection<SpaccioUser>? SpaccioUsers { get; init; }
}

public class SpaccioUser
{
    public string? UserName { get; init; }

    public string? Email { get; init; }

    public string? Password { get; init; }
}
