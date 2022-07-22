﻿using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SpaccioDescans.Core.Application.Orders.Commands;
using Syncfusion.Blazor.Notifications;

namespace SpaccioDescans.Web.Pages.Orders.Create.Components;

public class OrderCreateBase : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    protected OrderViewModel OrderViewModel { get; private set; } = new();

    protected bool ConfirmDialogVisibility { get; set; }

    protected SfToast ConfirmToast { get; set; } = default!;

    protected bool ShowOrderItems => this.OrderViewModel.OrderDetailViewModels.Count > 0;

    protected void Submit()
    {
        this.ConfirmDialogVisibility = true;
    }

    protected void Clear()
    {
        this.OrderViewModel.CustomerInfoViewModel.Address = string.Empty;
        this.OrderViewModel.CustomerInfoViewModel.Phone = string.Empty;
        this.OrderViewModel.CustomerInfoViewModel.Nif = string.Empty;
        this.OrderViewModel.CustomerInfoViewModel.Name = string.Empty;
        this.OrderViewModel.CashAmount = 0;
        this.OrderViewModel.CreditCardAmount = 0;
        this.OrderViewModel.FinancedAmount = 0;
        this.OrderViewModel.OrderDetailViewModels.Clear();
    }

    protected void UpdateTotal(decimal total)
    {
        this.OrderViewModel.NetAmount = total;
    }

    protected async Task CreateOrderAsync()
    {
        this.ConfirmDialogVisibility = false;

        var command = (CreateOrderCommand)this.OrderViewModel;
        var orderId = await this.Mediator.Send(command);

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
        await this.JSRuntime.InvokeAsync<object>("open", url, "_blank");
    }
}