using MediatR;

namespace Helpers.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
