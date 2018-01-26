using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IzzyDevo.CQRS.ExternalServices.Santa.Contracts;
using IzzyDevo.CQRS.Infrastructure.Pipeline;
using IzzyDevo.CQRS.Infrastructure.Pipeline.Logging;
using MediatR;

namespace IzzyDevo.CQRS.Domain.Santa
{
    public class SendSantaWishlist : ICommand, ILogged
    {
        public IEnumerable<string> Wishes { get; set; }

        public string LogMessage { get; }
    }

    /// <summary>
    /// Handles <see cref="SendSantaWishlist"/> command.
    /// </summary>
    public class SendSantaWishlistHandler : IRequestHandler<SendSantaWishlist>
    {
        private readonly ISantaAPI _santaApi;

        public SendSantaWishlistHandler(ISantaAPI santaApi)
        {
            _santaApi = santaApi;
        }

        public async Task Handle(SendSantaWishlist message, CancellationToken cancellationToken)
        {
            var wishlist = message.Wishes.Select(Wish.Create).ToList();
            await _santaApi.SendWishlist(wishlist, cancellationToken);
        }
    }
}
