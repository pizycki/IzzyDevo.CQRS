using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using MediatR;

namespace IzzyDevo.CQRS.Infrastructure.Pipeline.Transactional
{
    public class RequestTransctionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is ITransactional)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // In inner handler you can open multiple sessions and call SaveChanges several times.
                    // All will be comitted once is transaction is completed.
                    var result = await next();
                    transaction.Complete();
                    return result;
                }
            }

            return await next();
        }
    }
}
