using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaccioDescans.Core.Stores;

namespace SpaccioDescans.Infrastructure.Persistence.Configurations;

public class StoreEntityTypeConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Navigation(x => x.ProductStores).AutoInclude();

        builder.HasData(new Store(1, "Tienda 1", "Carretera de Terrassa"), new Store(2, "Tienda 2", "Avenida de Matadepera"));
    }
}