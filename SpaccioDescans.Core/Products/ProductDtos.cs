namespace SpaccioDescans.Core.Products;

public class ProductDto
{
    public long Id { get; set; }

    public string Vendor { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Meassures { get; set; } = default!;

    public decimal NetPrice { get; set; }

    public ICollection<ProductStoreDto> ProductStores { get; set; } = new List<ProductStoreDto>();
}

public class ProductStoreDto
{
    public long StoreId { get; set; }

    public int Quantity { get; set; }
}