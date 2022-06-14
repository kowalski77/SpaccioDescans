using Microsoft.AspNetCore.Components;

namespace SpaccioDescans.Web.Pages.Orders.Create.Components;

public class Order2Base : ComponentBase
{
    protected OrderViewModel OrderViewModel { get; } = new();
}