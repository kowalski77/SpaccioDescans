namespace SpaccioDescans.Core.Products;

public sealed record ProductDto(Guid Id, string Name, string Description, string Measures, decimal NetPrice);