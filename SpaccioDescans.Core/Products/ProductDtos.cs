namespace SpaccioDescans.Core.Products;

public sealed record ProductDto(Guid Id, int Code, string Vendor,  string Name, string Description, string Measures, decimal NetPrice, IEnumerable<ProductStoreDto> ProductStoreDtos);

public sealed record ProductStoreDto(int StoreCode, int Quantity);