using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaccioDescans.Core.Domain.Stores;

namespace SpaccioDescans.Infrastructure.Persistence.Configurations;

public class StoreEntityTypeConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Navigation(x => x.ProductStores).AutoInclude();

        builder.HasData(
            new Store(1, "Tienda 1", "Yasmina Aranda Díaz", "47170665B", "spaccio.descanso@hotmail.com", "Carretera de Terrassa, 377 local 1, C.P. 08205 - Sabadell (Barcelona)"),
            new Store(2, "Tienda 2", "Esteban Aranda Manzano", "47170665B", "spaccio.descanso@hotmail.com", "Avenida de Matadepera, 171, C.P. 08207 - Sabadell (Barcelona)"));
    }
}