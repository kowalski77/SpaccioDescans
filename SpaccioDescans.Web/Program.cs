using MediatR;
using Microsoft.Extensions.Options;
using Radzen;
using SpaccioDescans.Core;
using SpaccioDescans.Core.Application.Products;
using SpaccioDescans.Infrastructure;
using SpaccioDescans.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication("Identity.Application").AddCookie();

// UI
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<TenantConfiguration>(builder.Configuration.GetSection(nameof(TenantConfiguration)));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<TenantConfiguration>>().Value);

// BE
builder.Services.AddMediatR(typeof(CreateProductCommand).Assembly);
builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

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