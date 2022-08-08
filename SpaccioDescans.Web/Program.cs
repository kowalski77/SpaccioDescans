using MediatR;
using SpaccioDescans.Core;
using SpaccioDescans.Core.Domain.Products.Commands;
using SpaccioDescans.Infrastructure;
using SpaccioDescans.Web.Areas.Identity.Support;
using SpaccioDescans.Web.Invoices;
using SpaccioDescans.Web.Support;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
builder.Services.AddAuthentication("Identity.Application")
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
    });

// UI
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<MediatorFacade>();

// BE
builder.Services.AddInvoiceService(builder.Configuration);
builder.Services.AddMediatR(typeof(CreateProductCommand).Assembly);
builder.Services.AddCore(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["SyncfusionKey"]);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/Identity/Account/Register", context => Task.Run(() => context.Response.Redirect("/Identity/Account/Login", true, true)));
app.MapPost("/Identity/Account/Register", context => Task.Run(() => context.Response.Redirect("/Identity/Account/Login", true, true)));

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.SeedUsersAsync();

await app.RunAsync();