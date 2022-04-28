using MediatR;
using Radzen;
using SpaccioDescans.Core;
using SpaccioDescans.Core.Application;
using SpaccioDescans.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddAuthentication("Identity.Application").AddCookie();

// UI
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<DialogService>();

builder.Services.AddHttpContextAccessor();

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