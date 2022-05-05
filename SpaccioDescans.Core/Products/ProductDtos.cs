namespace SpaccioDescans.Core.Products;

public sealed record ProductDto(long Id, string Vendor,  string Name, string Description, string Measures, decimal NetPrice, IEnumerable<ProductStoreDto> ProductStoreDtos);

public sealed record ProductStoreDto(long StoreId, int Quantity);