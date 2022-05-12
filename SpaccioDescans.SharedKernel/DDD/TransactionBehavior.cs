using MediatR;
using Microsoft.EntityFrameworkCore;

namespace SpaccioDescans.SharedKernel.DDD;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IDbContext dbContext;

    public TransactionBehaviour(IDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(next);

        return this.HandleInternal(next, cancellationToken);
    }

    private async Task<TResponse> HandleInternal(RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var strategy = this.dbContext.DatabaseFacade.CreateExecutionStrategy();

        var response = await strategy.ExecuteAsync(async () => 
                await this.ExecuteTransactionAsync(next, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);

        return response ?? throw new InvalidOperationException("Response is null");
    }

    private async Task<TResponse> ExecuteTransactionAsync(RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await using var transaction = await this.dbContext.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        var response = await next().ConfigureAwait(false);
        await this.dbContext.CommitTransactionAsync(transaction, cancellationToken).ConfigureAwait(false);

        return response;
    }
}