using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using SpaccioDescans.Core.Domain.Products.Commands;
using SpaccioDescans.Core.Domain.Products.Queries;
using SpaccioDescans.Web.Support;
using Syncfusion.Blazor.Grids;
using Action = Syncfusion.Blazor.Grids.Action;

namespace SpaccioDescans.Web.Pages.Products.Inventory.Components;

public class ProductsBase : ComponentBase
{
    [Inject] private MediatorFacade MediatorFacade { get; set; } = default!;

    protected Collection<ProductViewModel>? Products { get; private set; }

    protected SfGrid<ProductViewModel> Grid { get; set; } = default!;

    protected async Task OnGridActionBeginHandler(ActionEventArgs<ProductViewModel> arg)
    {
        ArgumentNullException.ThrowIfNull(arg);

        switch (arg.RequestType)
        {
            case Action.Save:
                await this.SaveAsync(arg);
                break;
            case Action.Delete:
                await this.DeleteAsync(arg);
                break;
        }
    }

    private async Task DeleteAsync(ActionEventArgs<ProductViewModel> arg)
    {
        var command = new DeleteProductCommand(arg.Data.Id);
        await this.MediatorFacade.SendAsync(command);
    }

    private async Task SaveAsync(ActionEventArgs<ProductViewModel> arg)
    {
        if (arg.Action == "Add")
        {
            var command = (CreateProductCommand)arg.Data;
            var id = await this.MediatorFacade.SendAsync(command);

            arg.Data.Id = id;
        }
        else
        {
            var command = (EditProductCommand)arg.Data;
            await this.MediatorFacade.SendAsync(command);
        }
    }

    protected override async Task OnInitializedAsync()
    {
        var products = await this.MediatorFacade.SendAsync(new GetProductsQuery());
        var viewModels = products.Select(x => (ProductViewModel)x).ToList();

        this.Products = new Collection<ProductViewModel>(viewModels);
    }
}