using MediatR;

namespace IzzyDevo.CQRS.Infrastructure.Pipeline
{
    public interface ICommand : IRequest { }

    public interface IQuery<out TResponse> : IRequest<TResponse> { }

}
