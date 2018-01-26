using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IzzyDevo.CQRS.ExternalServices.Santa.Contracts;

namespace IzzyDevo.CQRS.ExternalServices.Santa
{
    public class SantaAPI : ISantaAPI
    {
        public async Task SendWishlist(List<Wish> wishes, CancellationToken cancellationToken)
        {
            await Task.Delay(2000, cancellationToken);
        }
    }
}