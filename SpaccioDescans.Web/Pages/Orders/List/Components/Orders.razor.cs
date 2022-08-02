using System.Collections.ObjectModel;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SpaccioDescans.Core.Application.Orders.Queries;
using SpaccioDescans.Core.Application.Services;
using SpaccioDescans.Web.Pages.Orders.ViewModels;
using SpaccioDescans.Web.Support;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.XlsIO;

namespace SpaccioDescans.Web.Pages.Orders.List.Components;

public class OrdersBase : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    protected Collection<OrderItemViewModel>? Orders { get; private set; }

    protected SfGrid<OrderItemViewModel> Grid { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var orders = await this.Mediator.Send(new GetOrdersQuery());
        var viewModels = orders.Select(x => (OrderItemViewModel)x).ToList();

        this.Orders = new Collection<OrderItemViewModel>(viewModels);
    }

    protected void NavigateToOrderEdit(long id)
    {
        this.NavigationManager.NavigateTo($"/orders/edit/{id}");
    }

    protected async Task ToolbarClickHandler(ClickEventArgs _)
    {
        await this.Grid.ExportToExcelAsync();
    }
}
