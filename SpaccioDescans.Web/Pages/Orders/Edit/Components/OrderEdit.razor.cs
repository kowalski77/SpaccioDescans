using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SpaccioDescans.Core.Domain.Orders;
using SpaccioDescans.Core.Domain.Orders.Commands;
using SpaccioDescans.Core.Domain.Orders.Queries;
using SpaccioDescans.Core.Domain.Stores;
using SpaccioDescans.Core.Invoices;
using SpaccioDescans.Web.Invoices;
using SpaccioDescans.Web.Pages.Orders.ViewModels;
using SpaccioDescans.Web.Shared;
using SpaccioDescans.Web.Support;
using Syncfusion.Blazor.Notifications;

namespace SpaccioDescans.Web.Pages.Orders.Edit.Components;

public class OrderEditBase : ComponentBase
{
    [Parameter] public long OrderId { get; set; }

    [CascadingParameter] public MainLayout MainLayout { get; set; } = default!;

    [Inject] private MediatorFacade Mediator { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    [Inject] private IStoreService StoreService { get; set; } = default!;

    [Inject] private InvoiceFactory InvoiceFactory { get; set; } = default!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    protected OrderViewModel OrderViewModel { get; private set; } = new();

    protected bool IsCustomerDataEditable { get; set; }

    protected bool IsPaymentEditable { get; set; }

    protected bool ConfirmDialogVisibility { get; set; }

    protected SfToast ResultToast { get; set; } = default!;

    protected bool ShowOrderItems => this.OrderViewModel.OrderDetail.Count > 0;

    protected bool IsCancelled => this.OrderViewModel.OrderStatus == OrderStatus.Cancelled;

    protected override async Task OnInitializedAsync()
    {
        var order = await this.Mediator.SendAsync(new GetOrderByIdQuery(this.OrderId));
        this.OrderViewModel = order.AsOrderViewModel();
    }

    protected async Task EditClientDataAsync()
    {
        this.MainLayout.StartSpinner();

        var command = this.OrderViewModel.AsEditCustomerInfoCommand();
        await this.Mediator.SendAsync(command);

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

        var command = this.OrderViewModel.AsEditPaymentCommand();
        await this.Mediator.SendAsync(command);

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
        await this.Mediator.SendAsync(command);

        this.MainLayout.StopSpinner();

        await this.ResultToast.ShowAsync(new ToastModel
        {
            Content = "Factura cancelada",
            Height = "20px"
        });

        this.NavigationManager.NavigateTo(this.NavigationManager.Uri, forceLoad: true);
    }

    protected async Task PrintDeliveryNoteAsync()
    {
        this.MainLayout.StartSpinner();

        var store = await this.GetStoreAsync();
        var invoiceInfo = InvoiceMappers.Map(this.OrderViewModel, store);

        var stream = this.InvoiceFactory.GetInvoice<DeliveryNote>(invoiceInfo);

        await this.JSRuntime.SaveAs($"albaran_{this.OrderId}.xls", stream.ToArray());

        this.MainLayout.StopSpinner();
    }

    protected async Task PrintCustomerInvoiceAsync()
    {
        this.MainLayout.StartSpinner();

        var store = await this.GetStoreAsync();
        var invoiceInfo = InvoiceMappers.Map(this.OrderViewModel, store);

        var stream = this.InvoiceFactory.GetInvoice<CustomerInvoice>(invoiceInfo);

        await this.JSRuntime.SaveAs($"factura_{this.OrderId}.xls", stream.ToArray());

        this.MainLayout.StopSpinner();
    }

    private void DisableEditOperations()
    {
        this.IsCustomerDataEditable = false;
        this.IsPaymentEditable = false;
    }

    private async Task<Store> GetStoreAsync()
    {
        var authenticationState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userName = authenticationState.User.Identity?.Name!;
        var store = await this.StoreService.GetStoreByUserAsync(userName);

        return store;
    }
}

