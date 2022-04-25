namespace SpaccioDescans.Core.Products;

public sealed record ProductDto(Guid Id, int Code, string Name, string Description, string Measures, decimal NetPrice, int Quantity);