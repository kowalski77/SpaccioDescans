using System.Collections.ObjectModel;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using SpaccioDescans.Core.Application;
using SpaccioDescans.Web.ViewModels;

namespace SpaccioDescans.Web.Components;

public class ProductsBase : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    protected Collection<ProductViewModel>? ProductViewModelsCollection { get; private set; }

    protected RadzenDataGrid<ProductViewModel> ProductViewModelsGrid { get; set; } = default!;

    protected ProductViewModel? NewlyProduct { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        var products = await this.Mediator.Send(new GetProductsQuery()).ConfigureAwait(true);
        var viewModels = products.Select(x => (ProductViewModel)x).ToList();

        this.ProductViewModelsCollection = new Collection<ProductViewModel>(viewModels);
    }

    protected async Task SaveProductAsync(ProductViewModel product)
    {
        if (product == this.NewlyProduct)
        {
            this.NewlyProduct = null;
        }

        await this.ProductViewModelsGrid.UpdateRow(product).ConfigureAwait(true);
    }

    protected void CancelEdit(ProductViewModel product)
    {
        if (product == this.NewlyProduct)
        {
            this.NewlyProduct = null;
        }

        this.ProductViewModelsGrid.CancelEditRow(product);
    }

    protected async Task AddProductAsync()
    {
        this.NewlyProduct = new ProductViewModel();
        await this.ProductViewModelsGrid.InsertRow(this.NewlyProduct).ConfigureAwait(true);
    }

    protected async Task OnProductAddAsync(ProductViewModel product)
    {
        var command = (CreateProductCommand)product;
        _ = await this.Mediator.Send(command);
    }
}