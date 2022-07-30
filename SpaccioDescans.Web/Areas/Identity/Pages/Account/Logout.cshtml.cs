// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SpaccioDescans.Web.Areas.Identity.Pages.Account;

[IgnoreAntiforgeryToken(Order = 1001)]
public class LogoutModel : PageModel
{
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly ILogger<LogoutModel> logger;

    public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
    {
        this.signInManager = signInManager;
        this.logger = logger;
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        await this.signInManager.SignOutAsync();
        this.logger.LogInformation("User logged out.");
        if (returnUrl != null)
        {
            return this.LocalRedirect(returnUrl);
        }
        else
        {
            return this.LocalRedirect("~/");
        }
    }
}
