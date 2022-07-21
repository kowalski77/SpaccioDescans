using Microsoft.AspNetCore.Components;

namespace SpaccioDescans.Web.Pages.Orders.Edit.Components;

public class OrderEditBase : ComponentBase
{
    [Parameter]
    public long OrderId { get; set; }

    protected override void OnInitialized()
    {
        var id = this.OrderId;
        base.OnInitialized();
    }
}

