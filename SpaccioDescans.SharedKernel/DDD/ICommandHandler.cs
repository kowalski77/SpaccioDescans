using MediatR;

namespace SpaccioDescans.SharedKernel.DDD;

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
}