using MediatR;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Core.Application.Products.Commands;

public sealed record DeleteProductCommand(long Id) : IRequest;

public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository productRepository;

    public DeleteProductHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var product = await this.productRepository.GetAsync(request.Id, cancellationToken);
        product!.SoftDeleted = true;

        await this.productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
    }
}