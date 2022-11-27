using MediatR;

namespace Helpers.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
