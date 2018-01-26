using System;
using System.Threading;
using System.Threading.Tasks;
using IzzyDevo.CQRS.Domain.YouTube.Entities;
using IzzyDevo.CQRS.Infrastructure.Pipeline;
using IzzyDevo.CQRS.Infrastructure.Pipeline.Transactional;
using MediatR;
using Raven.Client;

namespace IzzyDevo.CQRS.Domain.YouTube
{
    /// <summary>
    /// Used in creation of new YouTube subscribtion.
    /// </summary>
    public class Subscribe : ICommand, ITransactional
    {
        public string UserId { get; set; }
        public string ChannelId { get; set; }
    }

    public class SubscribeHandler : IRequestHandler<Subscribe>
    {
        private readonly IDocumentStore _documentStore;

        public SubscribeHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task Handle(Subscribe message, CancellationToken cancellationToken)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(new Subscription
                {
                    Id = Guid.NewGuid().ToString(),
                    ChannelId = message.ChannelId,
                    UserId = message.UserId
                }, cancellationToken);

                await session.SaveChangesAsync(cancellationToken);
            }
        }
    }
}