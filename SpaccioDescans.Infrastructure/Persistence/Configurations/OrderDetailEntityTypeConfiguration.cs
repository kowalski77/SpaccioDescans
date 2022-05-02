using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Infrastructure.Persistence.Configurations;

public class OrderDetailEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.OwnsOne(x => x.Quantity, y =>
        {
            y.Property(z => z.Value).HasColumnName(nameof(OrderDetail.Quantity));
            y.Property(z => z.Value).HasPrecision(10, 2);
        });
        builder.OwnsOne(x => x.Discount, y =>
        {
            y.Property(z => z.Value).HasColumnName(nameof(OrderDetail.Discount));
            y.Property(z => z.Value).HasPrecision(10, 2);
        });
        builder.Property(x => x.SubTotal).HasPrecision(10, 2);
        builder.HasQueryFilter(x => !x.SoftDeleted);

        builder.Ignore(x => x.SubTotal);
    }
}