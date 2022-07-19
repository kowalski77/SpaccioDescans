using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Infrastructure.Persistence.Configurations;

public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Amount).HasPrecision(10, 2);
        builder.Property(x => x.PaymentMethod).HasConversion<string>().HasMaxLength(50);
    }
}