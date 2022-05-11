using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Web.ViewModels;

public class ProductOutOfStockViewModel
{
    public long Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public int QuantityStoreOne { get; set; }

    public int QuantityStoreTwo { get; set; }

    public IEnumerable<long> RelatedOrderIds { get; set; } = new List<long>();

    public static explicit operator ProductOutOfStockViewModel(ProductOutOfStockDto source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ProductOutOfStockViewModel
        {
            Id = source.Id,
            Description = source.Description,
            QuantityStoreOne = source.ProductStoreDtos.First(x=>x.StoreId == 1).Quantity,
            QuantityStoreTwo = source.ProductStoreDtos.First(x=>x.StoreId == 2).Quantity,
            RelatedOrderIds = source.OrderIds
        };
    }
}