namespace SpaccioDescans.Core.Products;

public class ProductDto
{
    public long Id { get; init; }

    public string Vendor { get; init; } = default!;

    public string Name { get; init; } = default!;

    public string Description { get; init; } = default!;

    public string Meassures { get; init; } = default!;

    public decimal NetPrice { get; init; }

    public ICollection<ProductStoreDto> ProductStores { get; internal set; } = new List<ProductStoreDto>();
}

public class ProductStoreDto
{
    public long StoreId { get; init; }

    public int Quantity { get; init; }
}