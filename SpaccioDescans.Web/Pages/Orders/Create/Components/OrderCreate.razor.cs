﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SpaccioDescans.Web.Pages.Orders.ViewModels;
using SpaccioDescans.Web.Shared;
using SpaccioDescans.Web.Support;
using Syncfusion.Blazor.Notifications;

namespace SpaccioDescans.Web.Pages.Orders.Create.Components;

public class OrderCreateBase : ComponentBase
{
    [CascadingParameter] public MainLayout MainLayout { get; set; } = default!;

    [Inject] private MediatorFacade Mediator { get; set; } = default!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    protected OrderViewModel OrderViewModel { get; private set; } = new();

    protected bool ConfirmDialogVisibility { get; set; }

    protected SfToast ConfirmToast { get; set; } = default!;

    protected bool ShowOrderItems => this.OrderViewModel.OrderDetail.Count > 0;

    protected void Submit()
    {
        this.ConfirmDialogVisibility = true;
    }

    protected void Clear()
    {
        this.OrderViewModel.CustomerInfo.Address = string.Empty;
        this.OrderViewModel.CustomerInfo.Phone = string.Empty;
        this.OrderViewModel.CustomerInfo.Nif = string.Empty;
        this.OrderViewModel.CustomerInfo.Name = string.Empty;
        this.OrderViewModel.CashAmount = 0;
        this.OrderViewModel.CreditCardAmount = 0;
        this.OrderViewModel.FinancedAmount = 0;
        this.OrderViewModel.OrderDetail.Clear();
    }

    protected void UpdateTotal(decimal total)
    {
        this.OrderViewModel.NetAmount = total;
    }

    protected async Task CreateOrderAsync()
    {
        this.ConfirmDialogVisibility = false;

        this.MainLayout.StartSpinner();

        var authenticationState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userName = authenticationState.User.Identity?.Name!;

        var command = this.OrderViewModel.AsCreateOrderCommand(userName);
        var orderId = await this.Mediator.SendAsync(command);

        this.MainLayout.StopSpinner();
        this.Clear();
        await this.ShowOrderCreatedNotificationAsync(orderId);
    }

    private async Task ShowOrderCreatedNotificationAsync(long orderId)
    {
        await this.ConfirmToast.ShowAsync(new ToastModel
        {
            Content = $"Factura {orderId} creada",
            Height = "20px"
        });

        // TODO: config file
        var url = $"http://winapwbaxwsogtj/Reports/report/SpaccioDescans.Reports/OrderReport?OrderId={orderId}";

        //_ = Task.Run(async () =>
        //{
        //    await this.JSRuntime.InvokeAsync<object>("open", url, "_blank");
        //});
    }
}