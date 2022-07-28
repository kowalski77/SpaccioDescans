using MediatR;
using Microsoft.AspNetCore.Components;
using SpaccioDescans.Core.Application.Orders.Commands;
using SpaccioDescans.Core.Application.Orders.Queries;
using SpaccioDescans.Core.Domain.Orders;
using SpaccioDescans.Web.Pages.Orders.ViewModels;
using SpaccioDescans.Web.Shared;
using Syncfusion.Blazor.Notifications;

namespace SpaccioDescans.Web.Pages.Orders.Edit.Components;

public class OrderEditBase : ComponentBase
{
    [Parameter] public long OrderId { get; set; }

    [CascadingParameter] public MainLayout MainLayout { get; set; } = default!;

    [Inject] private IMediator Mediator { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    protected OrderViewModel OrderViewModel { get; private set; } = new();

    protected bool IsCustomerDataEditable { get; set; }

    protected bool IsPaymentEditable { get; set; }

    protected bool ConfirmDialogVisibility { get; set; }

    protected SfToast ResultToast { get; set; } = default!;

    protected bool ShowOrderItems => this.OrderViewModel.OrderDetail.Count > 0;

    protected bool IsCancelled => this.OrderViewModel.OrderStatus == OrderStatus.Cancelled;

    protected override async Task OnInitializedAsync()
    {
        var order = await this.Mediator.Send(new GetOrderByIdQuery(this.OrderId));
        this.OrderViewModel = order.AsOrderViewModel();
    }

    protected async Task EditClientDataAsync()
    {
        this.MainLayout.StartSpinner();

        var command = (EditCustomerInfoCommand)this.OrderViewModel;
        await this.Mediator.Send(command);

        this.MainLayout.StopSpinner();

        await this.ResultToast.ShowAsync(new ToastModel
        {
            Content = "Datos del cliente actualizados",
            Height = "20px"
        });

        this.DisableEditOperations();
    }

    protected async Task EditPaymentAsync()
    {
        this.MainLayout.StartSpinner();
        
        var command = (EditPaymentCommand)this.OrderViewModel;
        await this.Mediator.Send(command);

        this.MainLayout.StopSpinner();

        await this.ResultToast.ShowAsync(new ToastModel
        {
            Content = "Pagos actualizados",
            Height = "20px"
        });

        this.DisableEditOperations();
    }

    protected void ShowCancelOrderDialog()
    {
        this.ConfirmDialogVisibility = true;
    }

    protected async Task CancelOrderAsync()
    {
        this.ConfirmDialogVisibility = false;
        this.MainLayout.StartSpinner();

        var command = new CancelOrderCommand(this.OrderViewModel.Id);
        await this.Mediator.Send(command);

        this.MainLayout.StopSpinner();

        await this.ResultToast.ShowAsync(new ToastModel
        {
            Content = "Factura cancelada",
            Height = "20px"
        });

        this.NavigationManager.NavigateTo(this.NavigationManager.Uri, forceLoad: true);
    }

    private void DisableEditOperations()
    {
        this.IsCustomerDataEditable = false;
        this.IsPaymentEditable = false;
    }
}

