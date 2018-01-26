using MediatR;

namespace IzzyDevo.CQRS.Infrastructure.Pipeline.Logging
{
    public interface ILogged : IRequest
    {
        string LogMessage { get; }
    }
}
