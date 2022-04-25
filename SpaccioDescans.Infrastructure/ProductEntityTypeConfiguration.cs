using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Infrastructure;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OwnsOne(x => x.NetPrice, y =>
        {
            y.Property(z => z.Value).HasColumnName(nameof(Product.NetPrice));
            y.Property(z => z.Value).HasPrecision(10, 2);
        });

        builder.OwnsOne(x => x.Quantity, y =>
        {
            y.Property(z => z.Value).HasColumnName(nameof(Product.Quantity));
        });

        builder.Property(x => x.Code).ValueGeneratedOnAdd();
    }
}