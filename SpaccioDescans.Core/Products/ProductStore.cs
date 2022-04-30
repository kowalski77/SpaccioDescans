using SpaccioDescans.Core.Stores;

namespace SpaccioDescans.Core.Products;

public class ProductStore
{
    public Quantity Quantity { get; internal set; } = default!;

    public Guid ProductId { get; internal set; }

    public Product Product { get; internal set; } = default!;

    public Guid StoreId { get; internal set; }

    public Store Store { get; internal set; } = default!;
}