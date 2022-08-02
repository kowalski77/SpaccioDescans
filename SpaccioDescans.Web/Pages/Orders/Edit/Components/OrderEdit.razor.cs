﻿using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SpaccioDescans.Core.Application.Orders.Commands;
using SpaccioDescans.Core.Application.Orders.Queries;
using SpaccioDescans.Core.Application.Services;
using SpaccioDescans.Core.Domain.Orders;
using SpaccioDescans.Core.Domain.Stores;
using SpaccioDescans.Web.Pages.Orders.ViewModels;
using SpaccioDescans.Web.Shared;
using SpaccioDescans.Web.Support;
using Syncfusion.Blazor.Notifications;
using CustomerInfo = SpaccioDescans.Core.Application.Services.CustomerInfo;

namespace SpaccioDescans.Web.Pages.Orders.Edit.Components;

public class OrderEditBase : ComponentBase
{
    [Parameter] public long OrderId { get; set; }

    [CascadingParameter] public MainLayout MainLayout { get; set; } = default!;

    [Inject] private IMediator Mediator { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    
    [Inject] private IStoreService StoreService { get; set; } = default!;

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
        var order = await this.Mediator.Send(new GetOrderByIdQuery(this.OrderId));
        this.OrderViewModel = order.AsOrderViewModel();
    }

    protected async Task EditClientDataAsync()
    {
        this.MainLayout.StartSpinner();

        var command = this.OrderViewModel.AsEditCustomerInfoCommand();
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
        
        var command = this.OrderViewModel.AsEditPaymentCommand();
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

    protected async Task PrintInvoiceAsync()
    {
        var headerInfo = await this.GetHeaderInfoAsync();
        var customerInfo = this.GetCustomerInfo();
        var orderDetailInfo = this.GetOrderDetailInfo();

        var filePath = Path.Combine("Files", "invoices.xls");
        using var invoiceBuilder = InvoiceBuilder.Create(filePath, new DeliveryNoteParser());

        var stream = invoiceBuilder
            .AddHeader(headerInfo)
            .AddCustomer(customerInfo)
            .AddOrderDetails(orderDetailInfo)
            .Build();

        await this.JSRuntime.SaveAs($"factura_{this.OrderId}.xls", stream.ToArray());
    }

    private async Task<HeaderInfo> GetHeaderInfoAsync()
    {
        var authenticationState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userName = authenticationState.User.Identity?.Name!;
        var store = await this.StoreService.GetStoreByUserAsync(userName);

        return new HeaderInfo
        {
            Name = store.Owner,
            Address = store.Address,
            FiscalId = store.Nif,
            InvoiceId = this.OrderId
        };
    }

    private CustomerInfo GetCustomerInfo()
    {
        return new CustomerInfo
        {
            Nif = this.OrderViewModel.CustomerInfo.Nif,
            Name = this.OrderViewModel.CustomerInfo.Name,
            Address = this.OrderViewModel.CustomerInfo.Address,
            Phone = this.OrderViewModel.CustomerInfo.Phone
        };
    }

    private IEnumerable<OrderDetailInfo> GetOrderDetailInfo()
    {
        return this.OrderViewModel.OrderDetail.Select(detail => new OrderDetailInfo
        {
            Quantity = detail.Quantity,
            ProductDescription = detail.Name,
            Discount = (double)detail.Discount,
            NetPrice = (double)detail.Price,
            Total = (double)detail.Total
        });
    }

    private void DisableEditOperations()
    {
        this.IsCustomerDataEditable = false;
        this.IsPaymentEditable = false;
    }
}

