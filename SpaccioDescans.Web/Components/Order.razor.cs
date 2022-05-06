using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;
using SpaccioDescans.Core.Application.Orders;
using SpaccioDescans.Web.ViewModels;

namespace SpaccioDescans.Web.Components;

public class OrderBase : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Inject] private NotificationService NotificationService { get; set; } = default!;

    protected OrderViewModel OrderViewModel { get; } = new();

    protected async Task Submit(OrderViewModel model)
    {
        var command = (CreateOrderCommand)model;
        var result = await this.Mediator.Send(command);

        this.NotificationService.Notify(NotificationSeverity.Success, "Factura creada", $"nº: {result}");
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