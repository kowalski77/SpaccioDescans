using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Infrastructure.Persistence.Configurations;

public class ProductStoreEntityTypeConfiguration : IEntityTypeConfiguration<ProductStore>
{
    public void Configure(EntityTypeBuilder<ProductStore> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(x => new { x.ProductId, x.StoreId });

        builder.HasOne(x => x.Product)
            .WithMany(y => y.ProductStores)
            .HasForeignKey(x => x.ProductId)
            .IsRequired(false);

        builder.HasOne(x => x.Store)
            .WithMany(y => y.ProductStores)
            .HasForeignKey(x => x.StoreId)
            .IsRequired(false);
    }
}