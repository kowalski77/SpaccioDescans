using System.ComponentModel.DataAnnotations;

namespace SpaccioDescans.Web.Pages.Orders.ViewModels;

public class CustomerInfoViewModel
{
    [Required(ErrorMessage = "Nombre necesario.")]
    public string Name { get; set; } = default!;

    public string Address { get; set; } = default!;

    [Required(ErrorMessage = "NIF necesario.")]
    public string Nif { get; set; } = default!;

    public string Phone { get; set; } = default!;
}
