using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaccioDescans.Core.Domain.Products;

namespace SpaccioDescans.Infrastructure.Persistence.Configurations;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.OwnsOne(x => x.NetPrice, y =>
        {
            y.Property(z => z.Value).HasColumnName(nameof(Product.NetPrice));
            y.Property(z => z.Value).HasPrecision(10, 2);
        });

        builder.HasQueryFilter(x => !x.SoftDeleted);
    }
}