using System.Threading;
using System.Threading.Tasks;
using IzzyDevo.CQRS.Infrastructure.Log;
using MediatR;

namespace IzzyDevo.CQRS.Infrastructure.Pipeline.Logging
{
    public class RequestLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IConsoleLogger _log;

        public RequestLoggingBehavior(IConsoleLogger log)
        {
            _log = log;
        }

        public async Task<TResponse> Handle(
            TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) =>
            request is ILogged
                ? await LogAndGo((ILogged)request, next)
                : await next();

        private async Task<TResponse> LogAndGo(ILogged request, RequestHandlerDelegate<TResponse> next)
        {
            _log.Debug(request.LogMessage);
            return await next();
        }
    }
}