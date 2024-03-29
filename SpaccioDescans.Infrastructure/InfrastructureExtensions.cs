﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaccioDescans.Core.Domain.Orders;
using SpaccioDescans.Core.Domain.Products;
using SpaccioDescans.Core.Domain.Stores;
using SpaccioDescans.Infrastructure.Persistence;
using SpaccioDescans.Infrastructure.Persistence.Repositories;

namespace SpaccioDescans.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IStoreRepository, StoreRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<SpaccioContext>();

        services.AddDbContext<SpaccioContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
            });
        });
    }
}