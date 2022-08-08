using MediatR;

namespace SpaccioDescans.Web.Support;

public class MediatorFacade
{
    private readonly IServiceProvider serviceProvider;

    public MediatorFacade(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        using var scope = this.serviceProvider.CreateAsyncScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(request, cancellationToken);
    }
}
