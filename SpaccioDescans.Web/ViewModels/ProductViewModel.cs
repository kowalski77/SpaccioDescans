using System.ComponentModel.DataAnnotations;
using SpaccioDescans.Core.Application;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Web.ViewModels;

public class ProductViewModel
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public int Code { get; set; }

    [Required] public string Name { get; set; } = string.Empty;

    [Required] public string Description { get; set; } = string.Empty;

    [Required] public string Measures { get; set; } = string.Empty;

    [Required] [Range(1, 10000)] public decimal Price { get; set; }

    [Required] [Range(1, 100)] public int Quantity { get; set; }

    public static explicit operator ProductViewModel(ProductDto productDto)
    {
        return new ProductViewModel
        {
            Id = productDto.Id,  Code = productDto.Code, 
            Name = productDto.Name, Description = productDto.Description, 
            Measures = productDto.Measures, Price = productDto.NetPrice, Quantity = productDto.Quantity
        };
    }

    public static explicit operator CreateProductCommand(ProductViewModel viewModel)
    {
        return new CreateProductCommand(viewModel.Id, viewModel.Name, viewModel.Description, viewModel.Measures, viewModel.Price, viewModel.Quantity);
    }
}