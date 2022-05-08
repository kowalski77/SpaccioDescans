using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;
using SpaccioDescans.Core.Application.Orders;
using SpaccioDescans.Web.ViewModels;

namespace SpaccioDescans.Web.Components;

public class OrderBase : ComponentBase
{
    [Inject] private DialogService DialogService { get; set; } = default!;

    [Inject] private IMediator Mediator { get; set; } = default!;

    [Inject] private NotificationService NotificationService { get; set; } = default!;

    protected OrderViewModel OrderViewModel { get; } = new();

    protected async Task Submit(OrderViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var isConfirmed = await this.ConfirmOrderCreationAsync();
        if (isConfirmed)
        {
            var command = (CreateOrderCommand)model;
            var result = await this.Mediator.Send(command);

            this.NotificationService.Notify(NotificationSeverity.Success, "Factura creada", $"nº: {result}");
        }
        else
        {
            this.NotificationService.Notify(NotificationSeverity.Info, "Creación de factura cancelada");
        }
    }

    protected void Cancel()
    {
        //
    }

    protected void UpdateTotal(decimal total)
    {
        this.OrderViewModel.NetAmount = total;
    }

    private async Task<bool> ConfirmOrderCreationAsync()
    {
        var isConfirmed = await this.DialogService.Confirm("¿Estás seguro?", "Crear factura", new ConfirmOptions
        {
            OkButtonText = "Sí",
            CancelButtonText = "Cancelar"
        });

        if (isConfirmed == null)
        {
            return false;
        }

        return (bool)isConfirmed;
    }
}