using System.Threading;
using System.Threading.Tasks;
using IzzyDevo.CQRS.Infrastructure.Pipeline;
using MediatR;

namespace IzzyDevo.CQRS.Domain.Life
{
    public class WhatIsTheMeaningOfLife : IQuery<int>
    {
    }

    public class WhatIsTheMeaningOfLifeHandler : IRequestHandler<WhatIsTheMeaningOfLife, int>
    {
        public async Task<int> Handle(WhatIsTheMeaningOfLife message, CancellationToken cancellationToken)
        {
            await Task.Delay(500, cancellationToken); // Computation must takes some time..

            return 42;
        }
    }
}
