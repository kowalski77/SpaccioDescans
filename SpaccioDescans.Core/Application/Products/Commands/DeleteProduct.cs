using MediatR;
using SpaccioDescans.Core.Domain.Products;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Application.Products.Commands;

public sealed record DeleteProductCommand(long Id) : ICommand<Unit>;

public sealed class DeleteProductHandler : ICommandHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository productRepository;

    public DeleteProductHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var product = await this.productRepository.GetByIdAsync(request.Id, cancellationToken);
        product!.Delete();

        await this.productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
    }
}