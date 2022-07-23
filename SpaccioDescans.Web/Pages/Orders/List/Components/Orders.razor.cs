using System.Collections.ObjectModel;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SpaccioDescans.Core.Application.Orders.Queries;
using SpaccioDescans.Web.Pages.Orders.ViewModels;
using Syncfusion.Blazor.Grids;

namespace SpaccioDescans.Web.Pages.Orders.List.Components;

public class OrdersBase : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    protected Collection<OrderItemViewModel>? Orders { get; private set; }

    protected SfGrid<OrderItemViewModel> Grid { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var orders = await this.Mediator.Send(new GetOrdersQuery());
        var viewModels = orders.Select(x => (OrderItemViewModel)x).ToList();

        this.Orders = new Collection<OrderItemViewModel>(viewModels);
    }

    protected async Task ShowOrderAsync(long id)
    {
        // TODO: config file
        var url = $"http://winapwbaxwsogtj/Reports/report/SpaccioDescans.Reports/OrderReport?OrderId={id}";
        await this.JSRuntime.InvokeAsync<object>("open", url, "_blank");
    }

    protected void NavigateToOrderEdit(long id)
    {
        this.NavigationManager.NavigateTo($"/orders/edit/{id}");
    }
}

