﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaccioDescans.Core.Domain.Orders;

namespace SpaccioDescans.Infrastructure.Persistence.Configurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.SubTotal).HasPrecision(10, 2);
        builder.Property(x => x.Total).HasPrecision(10, 2);
        builder.Property(x => x.Pending).HasPrecision(10, 2);
        builder.Property(x => x.Remarks).IsUnicode(false);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);

        builder.HasMany(x => x.Payments).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}