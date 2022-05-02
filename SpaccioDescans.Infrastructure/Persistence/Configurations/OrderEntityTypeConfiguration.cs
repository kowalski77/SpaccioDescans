using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Infrastructure.Persistence.Configurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(x => x.OrderId).ValueGeneratedOnAdd();
        builder.Property(x => x.SubTotal).HasPrecision(10, 2);
        builder.Property(x => x.Total).HasPrecision(10, 2);
        builder.Property(x => x.Remarks).IsUnicode(false);
    }
}