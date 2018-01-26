using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IzzyDevo.CQRS.ExternalServices.Santa.Contracts
{
    public interface ISantaAPI
    {
        Task SendWishlist(List<Wish> wishes, CancellationToken cancellationToken);
    }
}