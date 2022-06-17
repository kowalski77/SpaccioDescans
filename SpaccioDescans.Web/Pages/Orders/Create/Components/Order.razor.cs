using MediatR;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Notifications;

namespace SpaccioDescans.Web.Pages.Orders.Create.Components;

public class OrderBase : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    protected OrderViewModel OrderViewModel { get; private set; } = new();

    protected bool ConfirmDialogVisibility { get; set; }

    protected SfToast ConfirmToast { get; set; } = default!;

    protected bool ShowOrderItems => this.OrderViewModel.OrderDetailViewModels.Count > 0;

    protected void Submit()
    {
        this.ConfirmDialogVisibility = true;
    }

    protected void Cancel()
    {
        this.OrderViewModel.CustomerInfoViewModel.Address = string.Empty;
        this.OrderViewModel.CustomerInfoViewModel.Phone = string.Empty;
        this.OrderViewModel.CustomerInfoViewModel.City = string.Empty;
        this.OrderViewModel.CustomerInfoViewModel.Name = string.Empty;
        this.OrderViewModel.OrderDetailViewModels.Clear();
    }

    protected void UpdateTotal(decimal total)
    {
        this.OrderViewModel.NetAmount = total;
    }

    public async Task CreateOrderAsync()
    {
        //var command = (CreateOrderCommand)model;
        //var orderId = await this.Mediator.Send(command);

        //this.NotificationService.Notify(NotificationSeverity.Success, "Factura creada", $"nº: {orderId}");
    }
}