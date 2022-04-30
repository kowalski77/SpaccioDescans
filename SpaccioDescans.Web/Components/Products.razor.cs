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
        var products = await this.Mediator.Send(new GetProductsQuery());
        var viewModels = products.Select(x => (ProductViewModel)x).ToList();

        this.ProductViewModelsCollection = new Collection<ProductViewModel>(viewModels);
    }

    protected async Task AddProductAsync()
    {
        this.NewlyProduct = new ProductViewModel();
        await this.ProductViewModelsGrid.InsertRow(this.NewlyProduct);
    }

    protected async Task EditProductAsync(ProductViewModel product)
    {
        await this.ProductViewModelsGrid.EditRow(product);
    }

    protected async Task OnProductEditAsync(ProductViewModel product)
    {
        var command = (EditProductCommand)product;
        _ = await this.Mediator.Send(command);
    }

    protected async Task OnProductAddAsync(ProductViewModel product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var command = (CreateProductCommand)product;
        var code = await this.Mediator.Send(command);

        product.ProductCode = code;
        await this.ProductViewModelsGrid.UpdateRow(product);
    }

    protected async Task DeleteProductAsync(ProductViewModel product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (product == this.NewlyProduct)
        {
            this.NewlyProduct = null;
        }

        var command = new DeleteProductCommand(product.Id);
        _ = await this.Mediator.Send(command);

        this.ProductViewModelsCollection?.Remove(product);
        await this.ProductViewModelsGrid.Reload().ConfigureAwait(true);
    }

    protected async Task SaveProductAsync(ProductViewModel product)
    {
        if (product == this.NewlyProduct)
        {
            this.NewlyProduct = null;
        }

        await this.ProductViewModelsGrid.UpdateRow(product);
    }

    protected void CancelEdit(ProductViewModel product)
    {
        if (product == this.NewlyProduct)
        {
            this.NewlyProduct = null;
        }

        this.ProductViewModelsGrid.CancelEditRow(product);
    }
}