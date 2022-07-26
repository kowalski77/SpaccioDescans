using MediatR;

namespace SpaccioDescans.SharedKernel.DDD;

public interface ICommand<out TCommand> : IRequest<TCommand>
{
}