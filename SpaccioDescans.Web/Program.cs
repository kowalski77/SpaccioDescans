using MediatR;
using Microsoft.Extensions.Options;
using SpaccioDescans.Core;
using SpaccioDescans.Core.Application.Products.Commands;
using SpaccioDescans.Infrastructure;
using SpaccioDescans.Infrastructure.Persistence;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
builder.Services.AddAuthentication("Identity.Application").AddCookie();

// UI
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSyncfusionBlazor();

builder.Services.Configure<TenantConfiguration>(builder.Configuration.GetSection(nameof(TenantConfiguration)));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<TenantConfiguration>>().Value);

// BE
builder.Services.AddMediatR(typeof(CreateProductCommand).Assembly);
builder.Services.AddCore();
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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();