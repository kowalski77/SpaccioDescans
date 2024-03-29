﻿#nullable disable

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaccioDescans.Core.Domain.Stores;

namespace SpaccioDescans.Web.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> logger;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IStoreRepository storeRepository;
    private readonly IStoreCache storeCache;

    public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger, IStoreRepository storeRepository, IStoreCache storeCache)
    {
        this.signInManager = signInManager;
        this.logger = logger;
        this.storeRepository = storeRepository;
        this.storeCache = storeCache;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = new();

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string ErrorMessage { get; set; }

    public async Task OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(this.ErrorMessage))
        {
            this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
        }

        returnUrl ??= this.Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        this.ReturnUrl = returnUrl;

        var stores = await this.storeRepository.GetAllAsync();
        var items = stores.Select(item => new SelectListItem(item.Name, item.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        this.Input.SelectListItems = new Collection<SelectListItem>(items);
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= this.Url.Content("~/");

        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        var result = await this.signInManager.PasswordSignInAsync(this.Input.UserName, this.Input.Password, this.Input.RememberMe, false);
        if (result.Succeeded)
        {
            this.storeCache.SetUserStore(this.Input.UserName, this.Input.StoreId);
            this.logger.LogInformation("User logged in.");
            return this.LocalRedirect(returnUrl);
        }

        if (result.IsLockedOut)
        {
            this.logger.LogWarning("User account locked out.");
            return this.RedirectToPage("./Lockout");
        }

        this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return this.Page();
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Display(Name = "Recordarme?")]
        public bool RememberMe { get; set; }

        public int StoreId { get; set; }

        [Display(Name = "Tienda")]
        public Collection<SelectListItem> SelectListItems { get; set; }
    }
}