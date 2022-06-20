using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Infrastructure.Persistence.Configurations;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Address).IsUnicode(false);
        builder.Property(x => x.Nif).IsUnicode(false);
        builder.Property(x => x.Phone).IsUnicode(false);
        builder.Property(x => x.Name).IsUnicode(false);
    }
}