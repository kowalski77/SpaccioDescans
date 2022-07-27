using MediatR;
using Microsoft.AspNetCore.Components;
using SpaccioDescans.Core.Application.Orders.Commands;
using SpaccioDescans.Core.Application.Orders.Queries;
using SpaccioDescans.Web.Pages.Orders.ViewModels;
using SpaccioDescans.Web.Shared;
using Syncfusion.Blazor.Notifications;

namespace SpaccioDescans.Web.Pages.Orders.Edit.Components;

public class OrderEditBase : ComponentBase
{
    [Parameter] public long OrderId { get; set; }

    [CascadingParameter] public MainLayout MainLayout { get; set; } = default!;

    [Inject] private IMediator Mediator { get; set; } = default!;

    protected OrderViewModel OrderViewModel { get; private set; } = new();

    protected bool IsCustomerDataEditable { get; set; }

    protected bool IsPaymentEditable { get; set; }

    protected SfToast ResultToast { get; set; } = default!;

    protected bool ShowOrderItems => this.OrderViewModel.OrderDetail.Count > 0;

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
        this.IsCustomerDataEditable = false;
        this.IsPaymentEditable = false;
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

        this.IsCustomerDataEditable = false;
        this.IsPaymentEditable = false;
    }
}

