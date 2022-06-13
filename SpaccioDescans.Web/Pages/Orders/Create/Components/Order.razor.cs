//using MediatR;
//using Microsoft.AspNetCore.Components;
//using Radzen;
//using SpaccioDescans.Core.Application.Orders.Commands;

//namespace SpaccioDescans.Web.Pages.Orders.Create.Components;

//public class OrderBase : ComponentBase
//{
//    [Inject] private DialogService DialogService { get; set; } = default!;

//    [Inject] private IMediator Mediator { get; set; } = default!;

//    [Inject] private NotificationService NotificationService { get; set; } = default!;

//    protected OrderViewModel OrderViewModel { get; } = new();

//    protected async Task Submit(OrderViewModel model)
//    {
//        ArgumentNullException.ThrowIfNull(model);

//        var isValid = this.ValidateOrderDetails();
//        if (!isValid)
//        {
//            return;
//        }

//        var isConfirmed = await this.ConfirmOrderCreationAsync();
//        if (isConfirmed)
//        {
//            await this.CreateOrderAsync(model);
//        }
//    }

//    protected void Cancel()
//    {
//        //
//    }

//    protected void UpdateTotal(decimal total)
//    {
//        this.OrderViewModel.NetAmount = total;
//    }

//    private bool ValidateOrderDetails()
//    {
//        if (this.OrderViewModel.OrderDetailViewModels.Count is not 0)
//        {
//            return true;
//        }

//        this.NotificationService.Notify(NotificationSeverity.Error, "No has añadido productos");
//        return false;
//    }

//    private async Task<bool> ConfirmOrderCreationAsync()
//    {
//        var isConfirmed = await this.DialogService.Confirm("¿Estás seguro?", "Crear factura", new ConfirmOptions
//        {
//            OkButtonText = "Sí",
//            CancelButtonText = "Cancelar"
//        });

//        if (isConfirmed != null && (bool)isConfirmed)
//        {
//            return (bool)isConfirmed;
//        }

//        this.NotificationService.Notify(NotificationSeverity.Info, "Creación de factura cancelada");
//        return false;
//    }

//    private async Task CreateOrderAsync(OrderViewModel model)
//    {
//        var command = (CreateOrderCommand)model;
//        var orderId = await this.Mediator.Send(command);

//        this.NotificationService.Notify(NotificationSeverity.Success, "Factura creada", $"nº: {orderId}");
//    }
//}