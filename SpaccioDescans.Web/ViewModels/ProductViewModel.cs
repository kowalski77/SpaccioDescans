using System.ComponentModel.DataAnnotations;
using SpaccioDescans.Core;
using SpaccioDescans.Core.Application;
using SpaccioDescans.Core.Application.Products;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Web.ViewModels;

public class ProductViewModel
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public int ProductCode { get; set; }

    [Required] public string Vendor { get; set; } = string.Empty;

    [Required] public string Name { get; set; } = string.Empty;

    [Required] public string Description { get; set; } = string.Empty;

    [Required] public string Measures { get; set; } = string.Empty;

    [Required] [Range(1, 10000)] public decimal Price { get; set; }

    [Required] [Range(0, 100)] public int QuantityStoreOne { get; set; }

    [Required] [Range(0, 100)] public int QuantityStoreTwo { get; set; }

    public static explicit operator ProductViewModel(ProductDto productDto)
    {
        ArgumentNullException.ThrowIfNull(productDto);

        return new ProductViewModel
        {
            Id = productDto.Id,  ProductCode = productDto.Code, 
            Vendor = productDto.Vendor, Name = productDto.Name, Description = productDto.Description, 
            Measures = productDto.Measures, Price = productDto.NetPrice, 
            QuantityStoreOne = productDto.ProductStoreDtos.First(x=>x.StoreCode == SpaccioConstants.StoreOneCode).Quantity,
            QuantityStoreTwo = productDto.ProductStoreDtos.First(x=>x.StoreCode == SpaccioConstants.StoreTwoCode).Quantity
        };
    }

    public static explicit operator CreateProductCommand(ProductViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        var storeQuantities = new List<StoreQuantity>(new[]
        {
            new StoreQuantity(SpaccioConstants.StoreOneCode, viewModel.QuantityStoreOne),
            new StoreQuantity(SpaccioConstants.StoreTwoCode, viewModel.QuantityStoreTwo)
        });

        return new CreateProductCommand(viewModel.Id, viewModel.Vendor, viewModel.Name, 
            viewModel.Description, viewModel.Measures, viewModel.Price, storeQuantities);
    }

    public static explicit operator EditProductCommand(ProductViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        var storeQuantities = new List<StoreQuantity>(new[]
        {
            new StoreQuantity(1, viewModel.QuantityStoreOne),
            new StoreQuantity(2, viewModel.QuantityStoreTwo)
        });

        return new EditProductCommand(viewModel.Id, viewModel.Vendor, viewModel.Name, 
            viewModel.Description, viewModel.Measures, viewModel.Price, storeQuantities);
    }
}