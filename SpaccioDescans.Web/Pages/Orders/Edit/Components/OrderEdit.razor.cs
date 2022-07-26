using MediatR;
using Microsoft.AspNetCore.Components;
using SpaccioDescans.Core.Application.Orders.Commands;
using SpaccioDescans.Core.Application.Orders.Queries;
using SpaccioDescans.Web.Pages.Orders.ViewModels;
using Syncfusion.Blazor.Notifications;

namespace SpaccioDescans.Web.Pages.Orders.Edit.Components;

public class OrderEditBase : ComponentBase
{
    [Parameter]
    public long OrderId { get; set; }

    [Inject] private IMediator Mediator { get; set; } = default!;

    protected OrderViewModel OrderViewModel { get; private set; } = new();

    protected bool IsEditable { get; set; }

    protected SfToast ResultToast { get; set; } = default!;

    protected bool ShowOrderItems => this.OrderViewModel.OrderDetail.Count > 0;

    protected override async Task OnInitializedAsync()
    {
        var order = await this.Mediator.Send(new GetOrderByIdQuery(this.OrderId));
        this.OrderViewModel = order.AsOrderViewModel();
    }

    protected async Task EditClientDataAsync()
    {
        var command = (EditCustomerInfoCommand)this.OrderViewModel;
        await this.Mediator.Send(command);

        await this.ResultToast.ShowAsync(new ToastModel 
        {
            Content = "Datos del cliente actualizados",
            Height = "20px"
        });
        this.IsEditable = false;
    }

    protected Task EditPaymentAsync()
    {
        return Task.CompletedTask;
    }
}

