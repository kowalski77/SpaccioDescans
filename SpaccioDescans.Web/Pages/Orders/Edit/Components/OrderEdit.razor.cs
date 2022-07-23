using MediatR;
using Microsoft.AspNetCore.Components;
using SpaccioDescans.Core.Application.Orders.Queries;
using SpaccioDescans.Web.Pages.Orders.ViewModels;

namespace SpaccioDescans.Web.Pages.Orders.Edit.Components;

public class OrderEditBase : ComponentBase
{
    [Parameter]
    public long OrderId { get; set; }

    [Inject] private IMediator Mediator { get; set; } = default!;

    protected OrderViewModel OrderViewModel { get; private set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var order = await this.Mediator.Send(new GetOrderByIdQuery(this.OrderId));
        this.OrderViewModel = order.AsOrderViewModel();
    }

    protected void Submit()
    {
        
    }
}

