using System.Collections.ObjectModel;
using MediatR;
using Microsoft.AspNetCore.Components;
using SpaccioDescans.Core.Application.Orders.Queries;
using Syncfusion.Blazor.Grids;

namespace SpaccioDescans.Web.Pages.Orders.List.Components;

public class OrdersBase : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    protected Collection<OrderViewModel>? Orders { get; private set; }

    protected SfGrid<OrderViewModel> Grid { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var orders = await this.Mediator.Send(new GetOrdersQuery());
        var viewModels = orders.Select(x => (OrderViewModel)x).ToList();

        this.Orders = new Collection<OrderViewModel>(viewModels);
    }

    protected Task ShowOrderAsync(long id)
    {
        return Task.CompletedTask;
    }
}

