using MediatR;
using Microsoft.AspNetCore.Components;
using SpaccioDescans.Web.ViewModels;

namespace SpaccioDescans.Web.Components;

public class OrderBase : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    protected OrderViewModel OrderViewModel { get; } = new();

    protected void Submit(OrderViewModel arg)
    {
        //
    }

    protected void Cancel()
    {
        //
    }

    protected void UpdateTotal(decimal total)
    {
        this.OrderViewModel.NetAmount = total;
    }
}