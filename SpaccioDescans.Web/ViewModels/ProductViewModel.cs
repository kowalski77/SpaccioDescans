using System.ComponentModel.DataAnnotations;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Web.ViewModels;

public class ProductViewModel
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required] public string Name { get; set; } = string.Empty;

    [Required] public string Description { get; set; } = string.Empty;

    [Required] public string Measures { get; set; } = string.Empty;

    [Required] [Range(1, 10000)] public decimal Price { get; set; }

    public static explicit operator ProductViewModel(ProductDto productDto)
    {
        return new ProductViewModel
        {
            Id = productDto.Id, Name = productDto.Name, Description = productDto.Description, Measures = productDto.Measures, Price = productDto.NetPrice
        };
    }
}