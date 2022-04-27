using Blazored.LocalStorage;
using MediatR;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Radzen;
using SpaccioDescans.Core;
using SpaccioDescans.Core.Application;
using SpaccioDescans.Infrastructure;
using SpaccioDescans.Infrastructure.Persistence;
using SpaccioDescans.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

// UI
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<DialogService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IStoreSelector, StoreSelector>();
builder.Services.AddScoped<ITenantService, TenantService>();

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